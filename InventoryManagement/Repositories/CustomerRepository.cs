using InventoryManagement.DbContexts;
using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.Repositories
{
    public class CustomerRepository: Repository<Customer>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        public CustomerRepository(ApplicationDbContext dbContext, ILogger logger):base(dbContext, logger)
        {
            _logger = logger;
        }

        protected override Customer MapToEntity(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                CustomerMobile = reader["CustomerMobile"].ToString(),
                RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"])
            };
        }

        protected override void AddParameters(SqlCommand command, Customer entity)
        {
            command.Parameters.Add(new SqlParameter("@CustomerName", entity.CustomerName));
            command.Parameters.Add(new SqlParameter("@CustomerMobile", entity.CustomerMobile));
            command.Parameters.Add(new SqlParameter("@RegistrationDate", entity.RegistrationDate));
        }

        public async Task<(int Result, string Message)> AddCustomerAsync(Customer customer)
        {
            try
            {
                using (var cmd = CreateCommand("dbo.InsertCustomer"))
                {
                    AddParameters(cmd, customer);
                    await CheckConnectionOpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (
                                Result: Convert.ToInt32(reader["Result"]),
                                Message: reader["Message"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error inserting customer", ex);
                throw;
            }

            return (-1, "An unexpected error occurred.");
        }
    }
}