using InventoryManagement.DTOs;
using InventoryManagement.Models;
using InventoryManagement.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger _logger;

        public ProductService(ProductRepository productRepository, ILogger logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task AddProductAsync(Product product)
        {
            product.EntryDate = DateTime.Now;
            await _productRepository.AddAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                ProductID = p.ProductID,
                SerialNumber = p.SerialNumber,
                Name = p.Name,
                Quantity = p.Quantity,
                EntryDate = p.EntryDate.ToString("dd/MM/yyyy"),
                Price = p.Price,
                WarrantyDays = p.WarrantyDays,
                Category = p.Category,
                VendorID = p.VendorID,
                VendorName = p.VendorName,
            }).ToList();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

    }
}