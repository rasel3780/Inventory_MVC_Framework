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
        private readonly ILogger _logger;
        public string UserName { get; set; }
        public string Password { get; set; }

     
        public bool VerifyLogin()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

                SqlConnection _connection = new SqlConnection(conString);
                _connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = "dbo.spInventory_GetMembers";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@UserName", this.UserName));
                cmd.Parameters.Add(new SqlParameter("@Password", this.Password));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataTable);
                cmd.Dispose();
                _connection.Close();

                if(dataTable.Rows.Count>0)
                {
                    Log.Information("Query in database successful");
                    return true;
                }

                //var pdata = (from p in dataTable.AsEnumerable()
                //             where p.Field<string>("UserName") == this.UserName && p.Field<string>("Password") == this.Password
                //             select new
                //             {
                //                 UserName = p.Field<string>("UserName")
                //             }).SingleOrDefault();
                //if (this.UserName == "Rasel" && this.Password == "123456")
                //{
                //    return true;
                //}   
            }
            catch(Exception ex) 
            {
                Log.Error(ex.ToString());
            }
            return false;
        }
    }
}