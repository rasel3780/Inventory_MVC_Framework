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
        public ActionResult Login(string btnSubmit, string txtUsername, string txtPassword)
        {
            string LoginMsg = "";
            if(txtUsername == "Rasel" && txtPassword == "123456")
            {
                Session["User"] = "Rasel";
                return RedirectToAction("Dashboard","Home");
            }
            else
            {
                LoginMsg = "Faild, Username/Password not match";
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
            Session["User"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}