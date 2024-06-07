using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class InvoiceModel
    {
        public List<CartItem> CartItems { get; set; }
        public List<Customer> Customers { get; set; }
        public int SelectedCustomerId { get; set; }
        public Customer NewCustomer { get; set; }
    }
}