using Autofac.Diagnostics;
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
    public class CustomerController : Controller
    {

        private readonly CustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public CustomerController(CustomerRepository customerRepository, ILogger logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LstCustomer()
        {
            try
            {
                var customerList = await _customerRepository.GetAllAsync();
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error("There was a problem fetching customer list "+ex.Message);
                return Json(new { suceess = false, message = "Customer list fetching failed" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(string CustomerName, string CustomerMobile)
        {
            try
            {
                Customer newCustomer = new Customer
                {
                    CustomerName = CustomerName,
                    CustomerMobile = CustomerMobile,
                    RegistrationDate = DateTime.Now
                };

               
                var (result, message) = await _customerRepository.AddCustomerAsync(newCustomer);


                if (result == 1)
                {
                    return Json(new { success = true, message = "Customer added successfully!" });
                }
                else if (result == -1)
                {
                    return Json(new { success = false, message = message });
                }
                else
                {
                    return Json(new { success = false, message = "An unexpected error occurred." });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while adding a new customer.");
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }
    }
}