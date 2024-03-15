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
                List<Equipment> equipmentDataList = Equipment.GetEquipmentData();
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


            List<Equipment> equipmentDataList = Equipment.GetEquipmentData();
            ViewBag.equipmentDataList = equipmentDataList;
            ViewBag.equipmentTxt = "";

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
    }
}