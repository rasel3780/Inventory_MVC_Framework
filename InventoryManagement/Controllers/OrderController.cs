using InventoryManagement.Models;
using InventoryManagement.Repositories;
using InventoryManagement.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ILogger _logger;
        public OrderController(OrderService orderService, ILogger logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]   
        public async Task<ActionResult> GetOrderHistory()
        {
            try
            {
                var orderList = await _orderService.GetOrderHistoryAsync();
                
                return Json(orderList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching order history", ex.Message);
                return Json(new { success=false, message="Error fetching the order history " });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmOrder(int customerId, List<CartItem> cartItems)
        {
            try
            {
                Employee employee = new Employee();
                string user = Session["User"].ToString();
                int userId = employee.GetEmployeeByUserName(user);

                bool isOrderConfirmed = await _orderService.ConfirmOrderAsync(customerId, cartItems, userId);
                if (isOrderConfirmed)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to place the order. Please try again." });
                }
            }
            catch (Exception ex)
            {
                _logger.Information("Order placed fail" + ex.Message);
                return Json(new { success = false, message = "An error occurred while processing the order." });

            }
        }

        [HttpPost]
        public ActionResult ProceedToCheckout(List<CartItem> cart)
        {

            return Json(new { success = true, message = "Checkout successful!" });
        }
    }
}