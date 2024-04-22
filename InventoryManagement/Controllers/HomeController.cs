using InventoryManagement.Models;
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
                List<Equipment> equipmentDataList = Equipment.GetEquipmentData();
                DataTable dtCusTEquip = Customer.GetCustomerEquipmentAssignmentData();
                ViewBag.dtCusTEquip = dtCusTEquip;
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
        public ActionResult Dashboard(FormCollection frm, string btnSubmit)
        {

            // Add New Equipment 
            if (btnSubmit == "Save Equipment")
            {
                Equipment equipment = new Equipment();
                equipment.Name = frm["ddlEquipmentName"].ToString();
                equipment.EqCount = Convert.ToInt32(frm["txtQuantity"].ToString());
                equipment.EntryDate = Convert.ToDateTime(frm["txtEntryDate"].ToString());

                int returnResult = equipment.SaveEquipment();
                if(returnResult>0)
                {
                    ViewBag.OperationResult = "Saved Successfully";
                }

            }
            if (btnSubmit == "Save Assignment")
            {

            }

                //Equipment list table
                List<Equipment> equipmentDataList = Equipment.GetEquipmentData();
            ViewBag.equipmentDataList = equipmentDataList;
            ViewBag.equipmentTxt = "";

            //Customer Equipment Assign List table
            DataTable dtCusTEquip = Customer.GetCustomerEquipmentAssignmentData();
            ViewBag.dtCusTEquip = dtCusTEquip;
            
            //Search box
            if (btnSubmit == "search")
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

        [HttpGet]
        public ActionResult LstEquipment()
        {
            List<Equipment> equipmentDataList = Equipment.GetEquipmentData();
            var eqpList = (from  equipment in equipmentDataList select
                           new {
                               EquipmentId = equipment.EquipmentId,
                               Name = equipment.Name,
                               EqCount = equipment.EqCount.ToString(),
                               EntryDate = equipment.EntryDate.ToString("dd/MM/yyyy")
                           }).ToList();
            return Json(eqpList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LstCustomer()
        {
            List<Customer> customerDataList = Customer.GetCustomerData();
            
            return Json(customerDataList, JsonRequestBehavior.AllowGet);
        }
    }
}