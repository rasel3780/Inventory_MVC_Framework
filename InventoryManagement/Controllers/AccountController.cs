using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class AccountController : Controller
    {
        
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string btnSubmit, Account account)
        {
            string LoginMsg = "";
            bool verifyStatus = account.VerifyLogin();

            if (btnSubmit == "Login")
            {
                if (verifyStatus)
                {

                    Session["User"] = account.UserName;
                    LoginMsg = "Login Success";
                    //return RedirectToAction("Dashboard","Home");
                }
                else
                {
                    LoginMsg = "Faild, Username/Password not match";
                }
            }
            ViewBag.LoginMsg = LoginMsg;
            return View();
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