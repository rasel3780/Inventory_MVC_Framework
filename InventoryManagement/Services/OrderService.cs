using InventoryManagement.DTOs;
using InventoryManagement.Models;
using InventoryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> ConfirmOrderAsync(int customerId, List<CartItem> items, int userId)
        {
            return await _orderRepository.ConfirmOrderAsync(customerId, items, userId);
        }

        public async Task<List<OrderDto>> GetOrderHistoryAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(order => new OrderDto
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                CustomerName = order.CustomerName,
                CustomerMobile = order.CustomerMobile,
                OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Amount = order.Amount,
                VendorName = order.VendorName,
                SerialNumber = order.SerialNumber, 
                SoldBy = order.SoldBy,
            }).ToList();
        }
    }
}