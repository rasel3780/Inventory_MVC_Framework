﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public int WarrantyDays { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string VendorName { get; set; }

        public decimal TotalPrice
        {
            get { return Quantity * Price; }
        }
    }
}