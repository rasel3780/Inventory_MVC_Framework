using InventoryManagement.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InventoryManagement.Repositories
{
    public class ReportRepository: IReportRrepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public decimal GetDailySales() => ExecuteScalar<decimal>("[dbo].[GetDailySales]");
        public decimal GetWeeklySales() => ExecuteScalar<decimal>("[dbo].[GetWeeklySales]");
        public decimal GetMonthlySales() => ExecuteScalar<decimal>("[dbo].[GetMonthlySales]");
        public decimal GetYearlySales() => ExecuteScalar<decimal>("[dbo].[GetYearlySales]");
        public int GetTotalProducts() => ExecuteScalar<int>("[dbo].[GetTotalProducts]");
        public int GetOutOfStockProducts() => ExecuteScalar<int>("[dbo].[GetOutOfStockProducts]");
        public int GetTotalCustomers() => ExecuteScalar<int>("[dbo].[GetTotalCustomers]");
        public int GetTotalEmployees() => ExecuteScalar<int>("[dbo].[GetTotalEmployees]");

        private T ExecuteScalar<T>(string storedProcedure)
        {
            using (var cmd = new SqlCommand(storedProcedure, _dbContext.Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (_dbContext.Connection.State != ConnectionState.Open)
                {
                    _dbContext.Connection.Open();
                }
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? (T)Convert.ChangeType(result, typeof(T)):default;
            }
        }
    }
}