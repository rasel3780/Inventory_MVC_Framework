using InventoryManagement.Models;
using InventoryManagement.Repositories;
using Serilog;
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

        [HttpGet]
        public async Task<ActionResult> LstProduct()
        {
            try
            {
                var productList = await _productRepository.GetAllAsync();

                var pdtList = productList.Select(product => new

                {
                    ProductID = product.ProductID,
                    SerialNumber = product.SerialNumber,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    EntryDate = product.EntryDate.ToString("dd/MM/yyyy"),
                    Price = product.Price,
                    WarrantyDays = product.WarrantyDays,
                    Category = product.Category,
                    VendorName = product.VendorName
                }).ToList();
                return Json(pdtList, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                Log.Error(ex.Message);
                return Json(new { success = false, message = "An error occurred" });
            }

        }
    }
    }