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
        public decimal WeeklySales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal YearlySales { get; set; }
        public int TotalProduct { get; set; }
        public int OutOfStock { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalEmployee { get; set; }

        public static decimal GetDailySales()
        {
            string conString = DbConnection.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetDailySales]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public static decimal GetWeeklySales()
        {
            string conString = DbConnection.GetConnectionString();
            using(SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetWeeklySales]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public static decimal GetMonthlySales()
        {
            string conString = DbConnection.GetConnectionString();
            using(SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetMonthlySales]", connection)
                {
                    CommandType= CommandType.StoredProcedure
                };
                connection.Open ();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value?Convert.ToDecimal(result) : 0;

            } 
        }

        public static decimal GetYearlySales()
        {
            string conString = DbConnection.GetConnectionString();
            using (SqlConnection connection = new SqlConnection (conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetYearlySales]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open ();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value?Convert.ToDecimal(result) : 0;
            }    
        }

        public static int GetOutOfStockProducts()
        {
            string conString = DbConnection.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetOutOfStockProducts]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        public static int GetTotalProducts()
        {
            string conString = DbConnection.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetTotalProducts]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open ();
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value?Convert.ToInt32(result) : 0;
            }
        }

        public static int GetTotalEmployees()
        {
            string conString = DbConnection.GetConnectionString();
            using (SqlConnection connection = new SqlConnection ( conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetTotalEmployees]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open ();
                var result = cmd.ExecuteScalar();  
                return result!= DBNull.Value?Convert.ToInt32 (result) : 0;
            }
        }

        public static int GetTotalCustomers()
        {
            string conString = DbConnection.GetConnectionString();
            using(SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetTotalCustomers]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open ();
                var result = cmd.ExecuteScalar();
                return result!=DBNull.Value?Convert.ToInt32(result): 0;
            }
        }
    }
}