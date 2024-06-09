using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Drawing.Printing;
using Serilog;
using System.Configuration;

namespace InventoryManagement.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public double Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string ProductName { get; set; }
        public string VendorName { get; set; }
        public string ProductSN { get; set; }
        public string SoldBy { get; set; }

        public static bool ConfirmOrder(int customerId, List<CartItem> items, int userId)
        {
            string conString = DbConnection.GetConnectionString();
            SqlConnection _connection  = new SqlConnection(conString);
            _connection.Open();
            int update = 0;
            int insert = 0;
            try
            {
                foreach(var item in items)
                {
                    SqlCommand updateCMD = new SqlCommand();
                    updateCMD.Connection = _connection;
                    updateCMD.CommandText = "dbo.UpdateProductQuantity";
                    updateCMD.Parameters.Clear();
                    updateCMD.CommandType = CommandType.StoredProcedure;
                    updateCMD.CommandTimeout = 0;
                    updateCMD.Parameters.Add(new SqlParameter("@ProductID", item.ProductID));
                    updateCMD.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                    update = updateCMD.ExecuteNonQuery();
                    updateCMD.Dispose();
                    SqlCommand insertCMD = new SqlCommand();
                    insertCMD.Connection = _connection;
                    insertCMD.CommandText = "dbo.InsertOrderHistory";
                    insertCMD.Parameters.Clear();
                    insertCMD.CommandType = CommandType.StoredProcedure;
                    insertCMD.CommandTimeout = 0;
                    insertCMD.Parameters.Add(new SqlParameter("@CustomerID", customerId));
                    insertCMD.Parameters.Add(new SqlParameter("@ProductID", item.ProductID));
                    insertCMD.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                    insertCMD.Parameters.Add(new SqlParameter("@OrderDate", DateTime.Now));
                    insertCMD.Parameters.Add(new SqlParameter("@Amount", item.Price * item.Quantity));
                    insertCMD.Parameters.Add(new SqlParameter("@SoldBy", userId));
                    var orderIdParam = new SqlParameter("@OrderID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    insertCMD.Parameters.Add(orderIdParam);
                    insert = insertCMD.ExecuteNonQuery();
                    insertCMD.Dispose();
                }
                _connection.Close();
                if(update!=0 && insert!=0)
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                Log.Error("an error occurred", ex);
            }
            return false;
        }

        public static List<Order> GetOrderHistory()
        {
            List<Order> orderList = new List<Order>();
            string conString = DbConnection.GetConnectionString();
            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "dbo.GetOrderHistory";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    Order obj = new Order();
                    obj.OrderID =Convert.ToInt32(reader["OrderID"]);
                    obj.CustomerName = reader["CustomerName"].ToString();
                    obj.CustomerMobile = reader["CustomerMobile"].ToString();
                    obj.ProductSN = reader["ProductSN"].ToString();
                    obj.ProductName = reader["ProductName"].ToString();
                    obj.VendorName = reader["VendorName"].ToString();
                    obj.OrderDate = Convert.ToDateTime(reader["OrderDate"].ToString());
                    obj.Amount = Convert.ToDouble(reader["Amount"].ToString());
                    orderList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return orderList;

        }

    }
}