using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["User"] != null)
            {
                List<Product> productList = Product.GetProductList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Dashboard(FormCollection frm, string btnSubmit)
        {
       
            // Add New Product 
            if (btnSubmit == "Save Product")
            {
                Product product = new Product();
                product.SerialNumber = frm["SerialNumber"].ToString(); 
                product.Name = frm["Name"].ToString();
                product.Quantity = Convert.ToInt32(frm["Quantity"].ToString());
                product.VendorID = Convert.ToInt32(frm["VendorID"].ToString()); ;
                product.EntryDate = Convert.ToDateTime(frm["EntryDate"].ToString()); 
                product.Price= Convert.ToInt32(frm["Price"].ToString());
                product.WarrantyDays = Convert.ToInt32(frm["WarrantyDays"].ToString());
                product.Category = frm["Category"].ToString();
         
                int result = product.AddProduct();
                if(result > 0)
                {
                    ViewBag.OperationResult = "Saved Successfully";
                }
            }
            if (btnSubmit == "AddToCart")
            {
                var productIdValue = frm["productId"];
                if (!string.IsNullOrEmpty(productIdValue))
                {
                    int productId;
                    if (int.TryParse(productIdValue, out productId))
                    {
                        // Add logic to handle adding the product to the cart
                        // You can store the product ID in the session or database to maintain the cart state
                        ViewBag.OperationResult = "Added to cart";
                    }
                    else
                    {
                        ViewBag.OperationResult = "Invalid product ID.";
                    }
                }
                else
                {
                    ViewBag.OperationResult = "No product selected.";
                }
            }



            return View();
        }

        private void AddToCart(int productId)
        {
            Product product = Product.GetProductList
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult LstProduct()
        {
            List<Product> productList = Product.GetProductList();
            var pdtList = (from  product in productList
                           select
                           new {
                               ProductID = product.ProductID,
                               SerialNumber = product.SerialNumber,
                               Name = product.Name,
                               Quantity = product.Quantity,
                               EntryDate = product.EntryDate.ToString("dd/MM/yyyy"),
                               Price = product.Price,
                               WarrantyDays = product.WarrantyDays,
                               Category = product.Category,
                               VendorName = product.VendorName
                           }).ToList();
            return Json(pdtList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LstCustomer()
        {
            List<Customer> customerDataList = Customer.GetCustomerData();
            
            return Json(customerDataList, JsonRequestBehavior.AllowGet);
        }
    }
}