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

        [HttpGet]
        public ActionResult AddProduct()
        {
            return PartialView("_PartialProductEntryPanel");
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = product.AddProduct();
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Product added successfully." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to add product." });
                    }
                }
                return Json(new { success = false, message = "Please fill all required fields." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
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
        public JsonResult ConfirmOrder(int customerId, List<CartItem> cartItems)
        {
            try
            {
                Employee employee = new Employee();
                string user = Session["User"].ToString();
                int userId = employee.GetEmployeeByUserName(user);

                bool isOrderConfirmed = Order.ConfirmOrder(customerId, cartItems, userId);
                if (isOrderConfirmed)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to place the order. Please try again." });
                }
            }
            catch(Exception ex)
            {
                Log.Information("Order placed fail"+ex.Message);
                return Json(new { success = false, message = "An error occurred while processing the order." });

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
