using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int GetEmployeeByUserName(string userName)
        {
            string conString = DbConnection.GetConnectionString();
            SqlConnection _connection = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;

            cmd.CommandText = "[dbo].[GetEmployeeByUserName]";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@UserName", userName));
            cmd.CommandTimeout = 0;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            
            if(dataTable.Rows.Count > 0)
            {
                Id = (int)dataTable.Rows[0]["Id"];
                return Id;
            }
            return -1;
        }


    }



}