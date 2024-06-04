using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace InventoryManagement.Models
{ 
    public class Vendor
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
    }
}