using InventoryManagement.DbContexts;
using InventoryManagement.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace InventoryManagement.Repositories
{
    public class OrderRepository: Repository<Order>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public OrderRepository(ApplicationDbContext dbContext, ILogger logger):base(dbContext, logger) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override Order MapToEntity(SqlDataReader reader)
        {
            return new Order
            {
                OrderID = Convert.ToInt32(reader["OrderID"]),
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                CustomerMobile = reader["CustomerMobile"].ToString(),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                ProductName = reader["ProductName"].ToString(),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Amount = Convert.ToDouble(reader["Amount"]),
                VendorName = reader["VendorName"].ToString(),
                SerialNumber = reader["SerialNumber"].ToString(),
                SoldBy = reader["SoldBy"].ToString()
            };
        }

        protected override void AddParameters(SqlCommand command, Order entity)
        {
            command.Parameters.Add(new SqlParameter("@OrderID", entity.OrderID));
            command.Parameters.Add(new SqlParameter("@CustomerID", entity.CustomerID));
            command.Parameters.Add(new SqlParameter("@OrderDate", entity.OrderDate));
            command.Parameters.Add(new SqlParameter("@TotalAmount", entity.Amount));
            command.Parameters.Add(new SqlParameter("@SoldById", entity.SoldBy));
        }

        public async Task<bool> ConfirmOrderAsync(int customerId, List<CartItem> items, int userId )
        {
            int update = 0;
            int insertOder = 0;
            int insertItem = 0;
            try
            {
                decimal totalAmount = 0;
                foreach (var item in items)
                {
                    totalAmount += item.Price * item.Quantity;
                }
                using (var insertOrderCmd = CreateCommand("dbo.InsertOrder"))
                {
                    insertOrderCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    insertOrderCmd.Parameters.Add(new SqlParameter("@CustomerID", customerId));
                    insertOrderCmd.Parameters.Add(new SqlParameter("@OrderDate", DateTime.Now));
                    insertOrderCmd.Parameters.Add(new SqlParameter("@TotalAmount", totalAmount));
                    insertOrderCmd.Parameters.Add(new SqlParameter("@SoldById", userId));
                    var orderIdParam = new SqlParameter("@OrderID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    insertOrderCmd.Parameters.Add(orderIdParam);

                    await CheckConnectionOpenAsync();

                    insertOder = await insertOrderCmd.ExecuteNonQueryAsync();
                    int orderId = Convert.ToInt32(orderIdParam.Value);

                    foreach (var item in items)
                    {
                        using (var insertItemCmd = CreateCommand("dbo.InsertOrderItem"))
                        {
                            insertItemCmd.Parameters.Add(new SqlParameter("@OrderID", orderId));
                            insertItemCmd.Parameters.Add(new SqlParameter("@ProductID", item.ProductID));
                            insertItemCmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                            insertItemCmd.Parameters.Add(new SqlParameter("@Amount", item.Price * item.Quantity));
                            insertItem = await insertItemCmd.ExecuteNonQueryAsync();
                        }

                        using (var updateCmd = CreateCommand("dbo.UpdateProductQuantity"))
                        {
                            updateCmd.Parameters.Add(new SqlParameter("@ProductID", item.ProductID));
                            updateCmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                            update = await updateCmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                if (update != 0 && insertOder != 0 && insertItem != 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in ConfirmOrderAsync", ex);
            }
            return false;
        }

        public async Task<List<Order>> GetOrderHistoryAsync()
        {
            var orderList = new List<Order>();
            try
            {
                using (var cmd =  CreateCommand("dbo.GetOrderHistory"))
                {
                    await CheckConnectionOpenAsync();

                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orderList.Add(MapToEntity(reader));
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                _logger.Error("Error retrieving order history", ex);
                throw;
            }
            return orderList;
        }
    }
}