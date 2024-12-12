using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DTOs
{
    public class ReportDto
    {
        public decimal DailySales { get; set; }
        public decimal WeeklySales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal YearlySales { get; set; }
        public int TotalProduct { get; set; }
        public int OutOfStock { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalEmployee { get; set; }
    }
}