using InventoryManagement.DTOs;
using InventoryManagement.Repositories;

namespace InventoryManagement.Services
{
    public class ReportService
    {
        private readonly IReportRrepository _reportRepository;
        public ReportService(IReportRrepository reportRrepository)
        {
            _reportRepository = reportRrepository;
        }

        public ReportDto GetDashboardReport()
        {
            return new ReportDto
            {
                DailySales = _reportRepository.GetDailySales(),
                WeeklySales = _reportRepository.GetWeeklySales(),
                MonthlySales = _reportRepository.GetMonthlySales(),
                YearlySales = _reportRepository.GetYearlySales(),
                TotalProduct = _reportRepository.GetTotalProducts(),
                OutOfStock = _reportRepository.GetOutOfStockProducts(),
                TotalCustomer = _reportRepository.GetTotalCustomers(),
                TotalEmployee = _reportRepository.GetTotalEmployees(),
            };
        }
    }
}