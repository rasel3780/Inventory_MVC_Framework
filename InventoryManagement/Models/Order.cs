using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Order
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }

        public List<Order> OrderList { get; set; }

        public Order() 
        {
            OrderList = new List<Order>();
        }

        public static List<Order> GetOrderList()
        {
            List<Order> orderDatalist = new List<Order>();
            for(int i=0; i<101; i++)
            {
                Order order = new Order();
                order.CustomerId = "c_" + i;
                order.OrderId = "#paK:00" + i;
                order.OrderDate = DateTime.Now;
                if (i % 2 == 0)
                    order.Status = "Processing";
                else
                    order.Status = "Delivered";
                orderDatalist.Add(order);
            }
            return orderDatalist;
        }
    }
}