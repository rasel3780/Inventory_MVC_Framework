using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace InventoryManagement.Models
{ 
    public class Vendor
    {
        public string SupplierName { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int Status { get; set; }

        public List<Vendor> VendorList { get; set; }

        public Vendor()
        {
            VendorList = new List<Vendor>();
        }

        public static List<Vendor> GetVendorData()
        {
            List<Vendor> vendordataList = new List<Vendor>();

            for(int i=0; i<101; i++)
            {
                Vendor vnd = new Vendor();
                vnd.SupplierName = "Vendor_"+i.ToString();
                vnd.ProductId = "#SN:000"+i.ToString();
                vnd.Quantity = i + 7;
                vnd.UnitPrice = i + 10;
                vnd.Status = i % 2;
                vendordataList.Add(vnd);
            }
            return vendordataList;
        }
    }
}