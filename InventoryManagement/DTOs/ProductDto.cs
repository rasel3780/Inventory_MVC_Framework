namespace InventoryManagement.DTOs
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string EntryDate { get; set; }
        public decimal Price { get; set; }
        public int WarrantyDays { get; set; }
        public string Category { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
    }
}