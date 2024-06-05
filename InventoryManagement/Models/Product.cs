using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Serilog;

namespace InventoryManagement.Models
{
    [Serializable]
    public class Product
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string SerialNumber {get; set;}
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public DateTime EntryDate { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public int WarrantyDays { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public int VendorID { get; set; } 

        public static List<Product> GetProductList()
        {
            List<Product> productList = new List<Product>();
            string conString = DbConnection.GetConnectionString();

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "dbo.GetProductList";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            SqlDataReader reader = cmd.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read()) 
                {
                    Product obj = new Product();
                    obj.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                    obj.SerialNumber = reader["SerialNumber"].ToString();
                    obj.Name = reader["Name"].ToString();
                    obj.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                    obj.EntryDate = Convert.ToDateTime(reader["EntryDate"].ToString());
                    obj.Price = Convert.ToDouble(reader["Price"].ToString());
                    obj.WarrantyDays = Convert.ToInt32(reader["WarrantyDays"].ToString());
                    obj.Category = reader["Category"].ToString();
                    obj.VendorName = reader["VendorName"].ToString() ;


                    productList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return productList;
        }

        public int AddProduct()
        {
            int result=0;
            try
            {
                string conString = DbConnection.GetConnectionString();

                SqlConnection _connection = new SqlConnection(conString);
                _connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = "[dbo].[AddProduct]";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;


                cmd.Parameters.Add(new SqlParameter("@SerialNumber", this.SerialNumber));
                cmd.Parameters.Add(new SqlParameter("@Name", this.Name));
                cmd.Parameters.Add(new SqlParameter("@Quantity", this.Quantity));
                cmd.Parameters.Add(new SqlParameter("@VendorID", this.VendorID));
                cmd.Parameters.Add(new SqlParameter("@EntryDate", this.EntryDate));
                cmd.Parameters.Add(new SqlParameter("@Price", this.Price));
                cmd.Parameters.Add(new SqlParameter("@WarrantyDays", this.WarrantyDays));
                cmd.Parameters.Add(new SqlParameter("@Category", this.Category));

                result = cmd.ExecuteNonQuery();
                cmd.Dispose();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Log.Information("There was a problem while inserting product"+ex.Message);
            }
            
            return result;
        }
    }
}