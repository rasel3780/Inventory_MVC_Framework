using InventoryManagement.Models;
using InventoryManagement.Services;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly ReportService _reportService;

        public HomeController(ReportService reportService, ILogger logger)
        {
            _reportService = reportService;
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
                var report = _reportService.GetDashboardReport();
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
