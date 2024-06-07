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
     
    }
}