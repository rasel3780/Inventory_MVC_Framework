using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DTOs
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string OrderDate { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; } 
        public string VendorName { get; set; } 
        public string SerialNumber { get; set; } 
        public string SoldBy { get; set; }
    }
}