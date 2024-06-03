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


        public int AddCustomer()
        {
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_InsertCustomer]";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@CustomerName", this.CustomerName));
            cmd.Parameters.Add(new SqlParameter("@CustomerMobile", this.CustomerMobile));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            _connection.Close();

            return result;
        }

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
 
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_LstCustomerEquiAssignment]";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;


            DataTable dataTable = new DataTable();    
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);


            cmd.Dispose();
            _connection.Close();
            return dataTable;
        }

        public static int SaveAssignment(int CustomerID, int EquipmentID, int EqQuantity)
        {
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_InsEquipmentAssignment]";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
            cmd.Parameters.Add(new SqlParameter("@EquipmentID", EquipmentID));
            cmd.Parameters.Add(new SqlParameter("@EqCount", EqQuantity));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            _connection.Close();

            return result;
        }
    }
}