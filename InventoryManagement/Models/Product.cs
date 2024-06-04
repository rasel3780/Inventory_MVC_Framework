using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual Vendor Vendor { get; set; }
        //public List<Product> ListProduct { get; set; }

        //public Product()
        //{
        //    ListProduct = new List<Product>();
        //}

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
                    //obj.Vendor.VendorID = Convert.ToInt32(reader["VendorID"].ToString());
                    obj.EntryDate = Convert.ToDateTime(reader["EntryDate"].ToString());
                    obj.Price = Convert.ToDouble(reader["Price"].ToString());
                    obj.WarrantyDays = Convert.ToInt32(reader["WarrantyDays"].ToString());
                    obj.Category = reader["Category"].ToString();
                   
                    productList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return productList;
        }

        //public int SaveProduct()
        //{
           
        //    string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

        //    SqlConnection _connection = new SqlConnection(conString);
        //    _connection.Open();

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = _connection;
        //    cmd.CommandText = "[dbo].[spInventory_InsertEquipment]";
        //    cmd.Parameters.Clear();

        //    cmd.Parameters.Add(new SqlParameter("@Name", this.Name));
        //    cmd.Parameters.Add(new SqlParameter("@EqCount", this.ProductCount));
        //    cmd.Parameters.Add(new SqlParameter("@EntryDate", this.EntryDate));

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandTimeout = 0;

        //    int result= cmd.ExecuteNonQuery();

        //    cmd.Dispose();
        //    _connection.Close();

        //    return result;
        //}
    }
}