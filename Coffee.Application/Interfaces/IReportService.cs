using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Application.Interfaces
{
    public interface IReportService
    {
        Task<decimal> GetDailyRevenueAsync(DateTime date);
        Task<decimal> GetMonthlyRevenueAsync(int year, int month);
        Task<decimal> GetYearlyRevenueAsync(int year);
        Task<Dictionary<string, int>> GetOrderStatusStatisticsAsync();
        Task<List<(string ProductName, string ImageUrl, int TotalSold, decimal Revenue)>> GetTopSellingProductsAsync(int count);
        Task<ItemStatsResult> GetItemStatsAsync();

        // New methods for custom date range reports
        Task<Dictionary<string, decimal>> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<(string ProductName, string ImageUrl, int TotalSold, decimal Revenue)>> GetTopSellingItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int count);
    }

    public class ItemStatItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int TotalSold { get; set; }
        public bool IsActive { get; set; }
    }

    public class ItemStatsResult
    {
        public List<ItemStatItem> Products { get; set; } = new();
        public List<ItemStatItem> Combos { get; set; } = new();
    }
}
