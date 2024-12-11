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
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly ILogger _logger;

        public OrderController(OrderRepository orderRepository, ILogger logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]   
        public async Task<ActionResult> GetOrderHistory()
        {
            try
            {
                var orderList = await _orderRepository.GetAllAsync();
                var orderDataList = orderList.Select(order => new

                {
                    OrderID = order.OrderID,
                    CustomerName = order.CustomerName,
                    CustomerMobile = order.CustomerMobile,
                    SerialNumber = order.SerialNumber,
                    ProductName = order.ProductName,
                    VendorName = order.VendorName,
                    OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                    Amount = order.Amount,
                    SoldBy = order.SoldBy
                }).ToList();
                return Json(orderDataList, JsonRequestBehavior.AllowGet);
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

                bool isOrderConfirmed = await _orderRepository.ConfirmOrderAsync(customerId, cartItems, userId);
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