using InventoryManagement.Models;
using System;
using System.Collections.Generic;
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
                //List<Employee> empListData = Employee.GetEmpData(); 
                //List<Vendor> vendorListData = Vendor.GetVendorData();
                //List<Order> orderListData = Order.GetOrderList();
                //ViewBag.vendorListData = vendorListData;
                //ViewBag.orderListData = orderListData;
                //ViewBag.orderTxt = "";
                List<Equipment> equipmentDataList = new List<Equipment>();
                ViewBag.equipmentDataList = equipmentDataList;
                ViewBag.equipmentTxt = "";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Dashboard(FormCollection frm, string filterBtn)
        {

            //List<Employee> empListData = Employee.GetEmpData();
            //List<Vendor> vendorListData = Vendor.GetVendorData();
            //List<Order> orderListData = Order.GetOrderList();

            //ViewBag.vendorListData = vendorListData;
            //ViewBag.orderListData = orderListData;
            //ViewBag.orderTxt = "";

            List<Equipment> equipmentDataList = new List<Equipment>();
            ViewBag.equipmentDataList = equipmentDataList;
            ViewBag.equipmentTxt = "";

            if (filterBtn == "search")
            {
                ViewBag.equipmentTxt = frm["equipmentTxt"].ToString();
            }
            return View();
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