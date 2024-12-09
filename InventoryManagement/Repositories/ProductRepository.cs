using InventoryManagement.DbContexts;
using InventoryManagement.Models;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Repositories
{
    public class ProductRepository: Repository<Product>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        public ProductRepository(ApplicationDbContext dbContext, ILogger logger):base(dbContext,logger)
        {
            _dbContext = dbContext;
            _logger = logger;
          
        }

        protected override Product MapToEntity(SqlDataReader reader)
        {
            return new Product
            {
                ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                SerialNumber = reader["SerialNumber"].ToString(),
                Name = reader["Name"].ToString(),
                Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                EntryDate = Convert.ToDateTime(reader["EntryDate"].ToString()),
                Price = Convert.ToDecimal(reader["Price"].ToString()),
                WarrantyDays = Convert.ToInt32(reader["WarrantyDays"].ToString()),
                Category = reader["Category"].ToString(),
                VendorName = reader["VendorName"].ToString()
            };
        }

        protected override void AddParameters(SqlCommand command, Product product)
        {
            command.Parameters.Add(new SqlParameter("@SerialNumber", product.SerialNumber));
            command.Parameters.Add(new SqlParameter("@Name", product.Name));
            command.Parameters.Add(new SqlParameter("@Quantity", product.Quantity));
            command.Parameters.Add(new SqlParameter("@VendorID", product.VendorID));
            command.Parameters.Add(new SqlParameter("@EntryDate", product.EntryDate));
            command.Parameters.Add(new SqlParameter("@Price", product.Price));
            command.Parameters.Add(new SqlParameter("@WarrantyDays", product.WarrantyDays));
            command.Parameters.Add(new SqlParameter("@Category", product.Category));
        }

        public async Task<Product> GetProductBySerialNumberAsync(string serialNumber)
        {
            try
            {
                using (var command = CreateCommand("dbo.GetProductBySerialNumber"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SerialNumber", serialNumber));

                    await CheckConnectionOpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapToEntity(reader);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error retrieving product by serial number", ex);
                throw;
            }
        }
    }
}