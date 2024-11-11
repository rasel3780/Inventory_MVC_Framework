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
using System.Runtime.Serialization;

namespace InventoryManagement.Models
{
    [Serializable]
    public class Order
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerMobile { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string SoldBy { get; set; }
       

        public static bool ConfirmOrder(int customerId, List<CartItem> items, int userId)
        {
            string conString = DbConnection.GetConnectionString();
            SqlConnection _connection  = new SqlConnection(conString);
            _connection.Open();
            int update = 0;
            int insertOder = 0;
            int insertItem = 0;
            try
            {
                decimal totatAmount = 0;
                foreach(var item in items)
                {
                    totatAmount += item.Price * item.Quantity;
                }
                SqlCommand insertCMD = new SqlCommand();
                insertCMD.Connection = _connection;
                insertCMD.CommandText = "dbo.InsertOrder";
                insertCMD.Parameters.Clear();
                insertCMD.CommandType = CommandType.StoredProcedure;
                insertCMD.CommandTimeout = 0;
                insertCMD.Parameters.Add(new SqlParameter("@CustomerID", customerId));
                insertCMD.Parameters.Add(new SqlParameter("@OrderDate", DateTime.Now));
                insertCMD.Parameters.Add(new SqlParameter("@TotalAmount", totatAmount));
                insertCMD.Parameters.Add(new SqlParameter("@SoldById", userId));
                var orderIdParam = new SqlParameter("@OrderID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                insertCMD.Parameters.Add(orderIdParam);
                insertOder = insertCMD.ExecuteNonQuery();
                int orderId = Convert.ToInt32(orderIdParam.Value);
                insertCMD.Dispose();
                foreach (var item in items)
                {
                    SqlCommand itemCMD = new SqlCommand();
                    itemCMD.Connection = _connection;
                    itemCMD.CommandText = "dbo.InsertOrderItem";
                    itemCMD.Parameters.Clear();
                    itemCMD.CommandType= CommandType.StoredProcedure;
                    itemCMD.CommandTimeout = 0;
                    itemCMD.Parameters.Add(new SqlParameter("@OrderID", orderId));
                    itemCMD.Parameters.Add(new SqlParameter("@ProductID", item.ProductID));
                    itemCMD.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                    itemCMD.Parameters.Add(new SqlParameter("@Amount", item.Price * item.Quantity));
                    insertItem = itemCMD.ExecuteNonQuery();



                    SqlCommand updateCMD = new SqlCommand();
                    updateCMD.Connection = _connection;
                    updateCMD.CommandText = "UpdateProductQuantity";
                    updateCMD.Parameters.Clear();
                    updateCMD.CommandType = CommandType.StoredProcedure;
                    updateCMD.CommandTimeout = 0;
                    updateCMD.Parameters.Add(new SqlParameter("@ProductID", item.ProductID));
                    updateCMD.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                    update = updateCMD.ExecuteNonQuery();
                    updateCMD.Dispose();

                    
                }
                _connection.Close();
                if(update!=0 && insertOder!=0 && insertItem!=0)
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
                    obj.OrderID =Convert.ToInt32(reader["OrderID"].ToString());
                    obj.CustomerName = reader["CustomerName"].ToString();
                    obj.CustomerMobile = reader["CustomerMobile"].ToString();
                    obj.SerialNumber = reader["SerialNumber"].ToString();
                    obj.ProductName = reader["ProductName"].ToString();
                    obj.VendorName = reader["VendorName"].ToString();
                    obj.OrderDate = Convert.ToDateTime(reader["OrderDate"].ToString());
                    obj.Amount = Convert.ToDouble(reader["Amount"].ToString());
                    obj.SoldBy = reader["SoldBy"].ToString() ;
                    orderList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return orderList;

        }

    }
}