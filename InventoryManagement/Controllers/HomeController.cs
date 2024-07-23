using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if (btnSubmit == "Save Product")
            {
                Product product = new Product();
                product.SerialNumber = frm["SerialNumber"].ToString();
                product.Name = frm["Name"].ToString();
                product.Quantity = Convert.ToInt32(frm["Quantity"].ToString());
                product.VendorID = Convert.ToInt32(frm["VendorID"].ToString());
                product.EntryDate = Convert.ToDateTime(frm["EntryDate"].ToString());
                product.Price = Convert.ToInt32(frm["Price"].ToString());
                product.WarrantyDays = Convert.ToInt32(frm["WarrantyDays"].ToString());
                product.Category = frm["Category"].ToString();

                int result = product.AddProduct();
                if (result > 0)
                {
                    ViewBag.OperationResult = "Saved Successfully";
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

            return Json(new
            {
                success = true,
                message = $"{product.Name} has been added to your cart.",
                product = product
            });
        }

        [HttpPost]
        public ActionResult UpdateCartQuantity(int productId, int change)
        {
            Product product = Product.GetProductById(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            Product product = Product.GetProductById(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ProceedToCheckout(List<CartItem> cart)
        {
            // Process the cart and create an invoice
            // For simplicity, just return a success response
            return Json(new { success = true, message = "Checkout successful!" });
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
            var pdtList = (from product in productList
                           select new
                           {
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
            List<Customer> customerList = Customer.GetCustomerList();
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Invoice()
        {
            var cartItems = Session["Cart"] as List<CartItem>;
            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
            }

            List<Customer> customerList = Customer.GetCustomerList().ToList();

            var model = new InvoiceModel
            {
                CartItems = cartItems,
                Customers = customerList
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCustomer(string CustomerName, string CustomerMobile)
        {
            try
            {
                Customer newCustomer = new Customer
                {
                    CustomerName = CustomerName,
                    CustomerMobile = CustomerMobile,
                    RegistrationDate = DateTime.Now
                };

                int result = newCustomer.AddCustomer();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a new customer.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ConfirmOrder(int customerId, List<CartItem> cartItems)
        {
            Employee employee = new Employee();
            string user = Session["User"].ToString();
            int userId = employee.GetEmployeeByUserName(user);

            var success = Order.ConfirmOrder(customerId, cartItems, userId);
            if (success)
            {
                Session["Cart"] = null;
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Failed to confirm order." });
            }
        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult GetOrderHistory()
        {
            List<Order> orderList = Order.GetOrderHistory();
            var orderDataList = (from order in orderList
                                 select new
                                 {
                                     OrderID = order.OrderID,
                                     CustomerName = order.CustomerName,
                                     CustomerMobile = order.CustomerMobile,
                                     SerialNumber = order.SerialNumber,
                                     ProductName = order.ProductName,
                                     VendorName = order.VendorName,
                                     OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                                     Amount = order.Amount,
                                     SoldBy = order.SoldBy
                                 }).ToList();
            return Json(orderDataList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProductById(int productId)
        {
            Product product = Product.GetProductById(productId);
            if (product != null)
            {
                return Json(product, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}
