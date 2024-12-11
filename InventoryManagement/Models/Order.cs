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

    }
}