using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
namespace InventoryManagement.Models
{
    public class Account
    {
        //private readonly ILogger _logger;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

       
        public bool VerifyLogin()
        {
           bool status = false;
            try
            {
                string conString = DbConnection.GetConnectionString();

                SqlConnection _connection = new SqlConnection(conString);
                _connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = "dbo.spInventory_GetMember";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@UserName", this.UserName));
                cmd.Parameters.Add(new SqlParameter("@Password", this.Password));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                if(dataTable.Rows.Count > 0)
                {
                    string userName = dataTable.Rows[0]["UserName"].ToString();
                    string password = dataTable.Rows[0]["Password"].ToString();
                    string role = dataTable.Rows[0]["Role"].ToString();
                    if (this.UserName==userName && this.Password==password)
                    {
                        this.Role = role;
                        Log.Information("User verifed");
                        status = true;
                    }
                }
                else
                {
                    Log.Warning("User verification failed");
                }
                cmd.Dispose();
                _connection.Close();
            }
            catch(Exception ex) 
            {
                Log.Error(ex, "An error occurred while verifying the user.");
            }
            return status;
        }
    }
}