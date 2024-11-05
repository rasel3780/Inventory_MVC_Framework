using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InventoryManagement.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            bool verifyStatus = account.VerifyLogin();

            if (verifyStatus)
            {

                Session["User"] = account.UserName;
                Session["Role"] = account.Role;
                
                Log.Information("Login success, redirecting to dashbord");

                return Json(new { success = true, redirectUrl = Url.Action("Dashboard", "Home") });
            }
            else
            {
                return Json(new { success = false } );
                
            }

           
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Log.Information("Logout");
            Session["User"] = null;

            return RedirectToAction("Login", "Account");
        }
    }
}