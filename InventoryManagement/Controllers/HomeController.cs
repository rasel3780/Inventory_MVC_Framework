using InventoryManagement.Models;
using InventoryManagement.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly ProductRepository _productRepository;

        public HomeController(ProductRepository productRepository, ILogger logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["User"] != null)
            {
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
        public ActionResult Invoice()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Customer> customerList = Customer.GetCustomerList().ToList();
            return View(); 
        }
    }
}
