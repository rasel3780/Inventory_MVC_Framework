using InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class VendorController : Controller
    {
        private readonly VendorService _vendorService;
        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        // GET: Vendor
        [HttpGet]
        public async Task<JsonResult> GetVendorList()
        {
            var vendors = await _vendorService.GetVendorListAsync();
            return Json(vendors, JsonRequestBehavior.AllowGet);
        }
        
    }
}