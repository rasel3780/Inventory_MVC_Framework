using InventoryManagement.DTOs;
using InventoryManagement.Models;
using InventoryManagement.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InventoryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountDto accountDto)
        {
            bool verifyStatus = _accountService.VerifyLogin(accountDto);

            if (verifyStatus)
            {

                Session["User"] = accountDto.UserName;
                Session["Role"] = accountDto.Role.ToString();
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