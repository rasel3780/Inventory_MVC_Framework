using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DTOs
{
    public class VendorDto
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
    }
}