using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositories
{
    public interface IReportRrepository
    {
        decimal GetDailySales();
        decimal GetWeeklySales();
        decimal GetMonthlySales();
        decimal GetYearlySales();
        int GetTotalProducts();
        int GetOutOfStockProducts();
        int GetTotalCustomers();
        int GetTotalEmployees();
    }
}
