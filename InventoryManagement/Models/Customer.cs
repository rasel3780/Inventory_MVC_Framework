using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }


        public static List<Customer> GetCustomerData()
        {
            List<Customer> customerList = new List<Customer>();
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_GetCustomers]";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Customer obj = new Customer();
                    obj.CustomerID = Convert.ToInt32(reader["CustomerID"].ToString());
                    obj.CustomerName = reader["CustomerName"].ToString();
                    obj.CustomerMobile = reader["CustomerMobile"].ToString();
                    customerList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return customerList;
        }

        public static DataTable GetCustomerEquipmentAssignmentData()
        {
            //List<Customer> customerList = new List<Customer>();
           
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_LstCustomerEquipmentAssignment]";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;


            DataTable dataTable = new DataTable();    
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            //SqlDataReader reader = cmd.ExecuteReader();

            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        Customer obj = new Customer();
            //        obj.CustomerID = Convert.ToInt32(reader["CustomerID"].ToString());
            //        obj.CustomerName = reader["CustomerName"].ToString();
            //        obj.CustomerMobile = reader["CustomerMobile"].ToString();
            //        customerList.Add(obj);
            //    }
            //}
            cmd.Dispose();
            _connection.Close();
            return dataTable;
        }
    }
}