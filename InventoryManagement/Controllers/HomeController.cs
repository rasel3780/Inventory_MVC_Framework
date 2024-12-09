using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

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
                Report report = new Report
                {
                    DailySales = Report.GetDailySales(),
                    WeeklySales = Report.GetWeeklySales(),
                    MonthlySales = Report.GetMonthlySales(),
                    YearlySales = Report.GetYearlySales(),
                    TotalProduct = Report.GetTotalProducts(),
                    OutOfStock = Report.GetOutOfStockProducts(),
                    TotalCustomer = Report.GetTotalCustomers(),
                    TotalEmployee = Report.GetTotalEmployees()
                };
                return View(report);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
        public ActionResult LstCustomer()
        {
            List<Customer> customerList = Customer.GetCustomerList();
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Invoice()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Customer> customerList = Customer.GetCustomerList().ToList();
            return View(); 
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

                string message;
                int result = newCustomer.AddCustomer(out message);


                if (result ==1 )
                {
                    return Json(new { success = true, message = "Customer added successfully!" });
                }
                else if (result == -1)
                {
                    return Json(new { success = false, message = message });
                }
                else
                {
                    return Json(new { success = false, message = "An unexpected error occurred." });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a new customer.");
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }

       

        [HttpPost]
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return Json(new { success = true });
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
