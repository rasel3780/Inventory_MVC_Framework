using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Serilog;
using System.Runtime.Serialization;

namespace InventoryManagement.Models
{
    [Serializable]
    public class Customer
    {
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerMobile { get; set; }
        [DataMember]
        public DateTime RegistrationDate { get; set; }

        public int AddCustomer()
        {
            string conString = DbConnection.GetConnectionString();
            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "dbo.InsertCustomer";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@CustomerName", this.CustomerName));
            cmd.Parameters.Add(new SqlParameter("@CustomerMobile", this.CustomerMobile));
            cmd.Parameters.Add(new SqlParameter("@RegistrationDate", this.RegistrationDate));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            _connection.Close();

            return result;
        }

        public static List<Customer> GetCustomerList()
        {
            List<Customer> customerList = new List<Customer>();
            string conString = DbConnection.GetConnectionString();

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[GetCustomerList]";
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

        public static Customer GetCustomerById(int customerId)
        {
            Customer customer = new Customer();
            try
            {
                string conString = DbConnection.GetConnectionString();

                SqlConnection _connection = new SqlConnection(conString);
                _connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = "dbo.GetCustomerById";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@@CustomerID", customerId));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    customer.CustomerID = Convert.ToInt32(dataTable.Rows[0]["CustomerID"]);
                    
                    customer.CustomerName = dataTable.Rows[0]["CustomerName"].ToString();
                    customer.CustomerMobile = dataTable.Rows[0]["CustomerMobile"].ToString();

                }
                else
                {
                    Log.Warning("Customer not found");
                }
                cmd.Dispose();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while tried to get the customer.");
            }
            return customer;
        }

    }
}