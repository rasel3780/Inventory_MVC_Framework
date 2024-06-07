using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Xml.Linq;
using static Azure.Core.HttpHeader;

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
                if (productIdValue!=null && productIdValue!="")
                {
                    int productId = Convert.ToInt32(productIdValue);
                    AddToCart(productId);
                }
                else
                {
                    ViewBag.OperationResult = "No product selected.";
                }
            }



            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            Product product = Product.GetProductById(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            List<CartItem> cart;

            if (Session["Cart"] == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = Session["Cart"] as List<CartItem>;
            }


            CartItem existingItem = null;
            foreach (var item in cart)
            {
                if (item.ProductID == productId)
                {
                    existingItem = item;
                    break;
                }
            }

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItem newItem = new CartItem();

                newItem.ProductID = productId;
                newItem.SerialNumber = product.SerialNumber;
                newItem.Name = product.Name;
                newItem.WarrantyDays = product.WarrantyDays;
                newItem.Quantity = 1;
                newItem.Price = product.Price;
                newItem.VendorName = product.VendorName;
                
                cart.Add(newItem);
            }

            Session["Cart"] = cart;
            return Json(new { success = true, cart = cart });
        }
        [HttpPost]
        public ActionResult UpdateCartQuantity(int productId, int change)
        {
            List<CartItem> cart;
            if (Session["Cart"] == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = Session["Cart"] as List<CartItem>;
            }
            CartItem itemToUpdate = null;
            foreach (var item in cart)
            {
                if (item.ProductID == productId)
                {
                    itemToUpdate = item;
                    break;
                }
            }

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity += change;
                if (itemToUpdate.Quantity<=0)
                {
                    cart.Remove(itemToUpdate);
                }
                

                Session["Cart"] = cart;
                return Json(new { success = true, cart = cart });
            }

            return Json(new { success = false, message = "Product not found in cart." });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            List<CartItem> cart;
            if (Session["Cart"] == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = Session["Cart"] as List<CartItem>;
            }
            CartItem itemToRemove = null;
            foreach (var item in cart)
            {
                if (item.ProductID == productId)
                {
                    itemToRemove = item;
                    break;
                }
            }
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                Session["Cart"] = cart;
                return Json(new { success = true, cart = cart });
            }

            return Json(new { success = false, message = "Product not found in cart." });
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

        [HttpGet]
        public ActionResult Invoice()
        {
            var cartItems = Session["Cart"] as List<CartItem>;
            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
            }

            var customers = Customer.GetCustomerData().ToList();
            var model = new InvoiceModel
            {
                CartItems = cartItems,
                Customers = customers
            };

            return View(model);
        }


    }
}