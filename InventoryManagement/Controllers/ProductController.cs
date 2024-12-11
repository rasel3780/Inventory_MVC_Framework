using InventoryManagement.Models;
using InventoryManagement.Services;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ILogger _logger;
        public ProductController(ProductService productService, ILogger logger)
        {
            _productService = productService;
            _logger = logger;
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
                    await _productService.AddProductAsync(product);
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
                var productList = await _productService.GetAllProductsAsync();
                return Json(productList, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                _logger.Error(ex.Message);
                return Json(new { success = false, message = "An error occurred" });
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            return Json(new
            {
                success = true,
                message = $"{product.Name} has been added to your cart.",
                product
            });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCartQuantity(int productId, int change)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<ActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product != null)
            {
                return Json(product, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }

}