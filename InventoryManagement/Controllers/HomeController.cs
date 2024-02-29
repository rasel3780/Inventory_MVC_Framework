using InventoryManagement.Models;
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
            List<Employee> empListData = Employee.GetEmpData(); 
            List<Vendor> vendorListData = Vendor.GetVendorData();
            List<Order> orderListData = Order.GetOrderList();
            ViewBag.vendorListData = vendorListData;
            ViewBag.orderListData = orderListData;
            return View(empListData);
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
    }
}