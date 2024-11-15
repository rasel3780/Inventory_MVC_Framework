using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Web;

namespace InventoryManagement.Models
{
    public class Report
    {
        public decimal DailySales { get; set; }
        public static decimal GetDailySales()
        {
            string conString = DbConnection.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetTodaysSale]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }
    }
}