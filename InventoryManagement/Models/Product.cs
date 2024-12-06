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
        public decimal Price { get; set; }
        [DataMember]
        public int WarrantyDays { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public int VendorID { get; set; } 

      

        //public int AddProduct()
        //{
        //    int result=0;
        //    try
        //    {
        //        string conString = DbConnection.GetConnectionString();

        //        SqlConnection _connection = new SqlConnection(conString);
        //        _connection.Open();

        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = _connection;
        //        cmd.CommandText = "[dbo].[InsertProduct]";
        //        cmd.Parameters.Clear();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;


        //        cmd.Parameters.Add(new SqlParameter("@SerialNumber", this.SerialNumber));
        //        cmd.Parameters.Add(new SqlParameter("@Name", this.Name));
        //        cmd.Parameters.Add(new SqlParameter("@Quantity", this.Quantity));
        //        cmd.Parameters.Add(new SqlParameter("@VendorID", this.VendorID));
        //        cmd.Parameters.Add(new SqlParameter("@EntryDate", this.EntryDate));
        //        cmd.Parameters.Add(new SqlParameter("@Price", this.Price));
        //        cmd.Parameters.Add(new SqlParameter("@WarrantyDays", this.WarrantyDays));
        //        cmd.Parameters.Add(new SqlParameter("@Category", this.Category));

        //        result = cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //        _connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("There was a problem while inserting product"+ex.Message);
        //    }
            
        //    return result;
        //}

        public static Product GetProductById(int productId)
        {
            Product product = new Product();
            try
            {
                string conString = DbConnection.GetConnectionString();

                SqlConnection _connection = new SqlConnection(conString);
                _connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _connection;
                cmd.CommandText = "dbo.GetProductById";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@ProductID", productId));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    product.ProductID = Convert.ToInt32(dataTable.Rows[0]["ProductID"]);
                    product.SerialNumber = dataTable.Rows[0]["SerialNumber"].ToString();
                    product.Name = dataTable.Rows[0]["Name"].ToString();
                    product.Quantity = Convert.ToInt32(dataTable.Rows[0]["Quantity"]);
                    product.EntryDate = Convert.ToDateTime(dataTable.Rows[0]["EntryDate"]);
                    product.Price = Convert.ToDecimal(dataTable.Rows[0]["Price"].ToString());
                    product.WarrantyDays = Convert.ToInt32(dataTable.Rows[0]["WarrantyDays"]);
                    product.Category = dataTable.Rows[0]["Category"].ToString();
                    product.VendorName = dataTable.Rows[0]["VendorName"].ToString();
                    product.VendorID = Convert.ToInt32(dataTable.Rows[0]["VendorID"]);
                }
                else
                {
                    Log.Warning("Product not found");
                }
                cmd.Dispose();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while tried to get the product.");
            }
            return product;
        }

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

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Product obj = new Product();
                    obj.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                    obj.SerialNumber = reader["SerialNumber"].ToString();
                    obj.Name = reader["Name"].ToString();
                    obj.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                    obj.EntryDate = Convert.ToDateTime(reader["EntryDate"].ToString());
                    obj.Price = Convert.ToDecimal(reader["Price"].ToString());
                    obj.WarrantyDays = Convert.ToInt32(reader["WarrantyDays"].ToString());
                    obj.Category = reader["Category"].ToString();
                    obj.VendorName = reader["VendorName"].ToString();


                    productList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return productList;
        }

    }
}