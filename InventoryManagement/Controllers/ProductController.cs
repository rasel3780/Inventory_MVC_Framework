using InventoryManagement.Models;
using InventoryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return PartialView("_PartialProductEntryPanel");
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productRepository.AddAsync(product);
                    return Json(new { success = true, message = "Product added successfully." });
                }
                return Json(new { success = false, message = "Please fill all the required filed" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occured:" + ex.Message });
            }
        }
    }
}