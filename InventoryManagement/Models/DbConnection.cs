using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class DbConnection
    {
        protected readonly SqlConnection _connection;
        
        public static string GetConnectionString()
        {
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;
            return conString;
        }
    }
}