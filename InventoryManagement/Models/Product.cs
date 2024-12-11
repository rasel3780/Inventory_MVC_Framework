using System;
using System.Runtime.Serialization;

namespace InventoryManagement.Models
{
    [Serializable]
    public class Product
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public DateTime EntryDate { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int WarrantyDays { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public int VendorID { get; set; }

    }
}