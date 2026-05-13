using System;
using System.Collections.Generic;
using System.Text;

using FastFood.Application.Interfaces;
using FastFood.Domain.Interfaces;

namespace FastFood.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IComboRepository _comboRepository;

        public ReportService(IOrderRepository orderRepository, IProductRepository productRepository, IComboRepository comboRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _comboRepository = comboRepository;
        }

        public async Task<decimal> GetDailyRevenueAsync(DateTime date)
        {
            var orders = await _orderRepository.GetOrdersByDateRangeAsync(
                date.Date,
                date.Date.AddDays(1)
            );

            return orders
                .Where(o => o.Status == "Completed")
                .Sum(o => o.TotalAmount);
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);

            return orders
                .Where(o => o.Status == "Completed")
                .Sum(o => o.TotalAmount);
        }

        public async Task<decimal> GetYearlyRevenueAsync(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year + 1, 1, 1);

            var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);

            return orders
                .Where(o => o.Status == "Completed")
                .Sum(o => o.TotalAmount);
        }

        public async Task<Dictionary<string, int>> GetOrderStatusStatisticsAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders
                .GroupBy(o => o.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<List<(string ProductName, string ImageUrl, int TotalSold, decimal Revenue)>> GetTopSellingProductsAsync(int count)
        {
            var orders = await _orderRepository.GetAllAsync();
            return GetTopItems(orders, count);
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate.AddDays(1).AddTicks(-1));
            
            return orders
                .Where(o => o.Status == "Completed")
                .GroupBy(o => o.CreatedAt.ToString("yyyy-MM-dd"))
                .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount));
        }

        public async Task<List<(string ProductName, string ImageUrl, int TotalSold, decimal Revenue)>> GetTopSellingItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int count)
        {
            var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate.AddDays(1).AddTicks(-1));
            return GetTopItems(orders, count);
        }

        private List<(string ProductName, string ImageUrl, int TotalSold, decimal Revenue)> GetTopItems(IEnumerable<FastFood.Domain.Entities.Order> orders, int count)
        {
            var itemStats = orders
                .Where(o => o.Status == "Completed")
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => new { oi.ProductId, oi.ComboId })
                .Select(g => new
                {
                    Key = g.Key,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.UnitPrice * oi.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .ToList();

            var result = new List<(string ProductName, string ImageUrl, int TotalSold, decimal Revenue)>();

            foreach (var stat in itemStats)
            {
                string name = "Unknown";
                string imageUrl = "";

                var sampleItem = orders
                    .SelectMany(o => o.OrderItems)
                    .FirstOrDefault(oi => oi.ProductId == stat.Key.ProductId && oi.ComboId == stat.Key.ComboId);

                if (sampleItem != null)
                {
                    if (sampleItem.ProductId.HasValue && sampleItem.Product != null)
                    {
                        name = sampleItem.Product.Name;
                        imageUrl = sampleItem.Product.ImageUrl ?? "";
                    }
                    else if (sampleItem.ComboId.HasValue && sampleItem.Combo != null)
                    {
                        name = sampleItem.Combo.Name;
                        imageUrl = sampleItem.Combo.ImageUrl ?? "";
                    }
                }

                result.Add((name, imageUrl, stat.TotalSold, stat.Revenue));
            }

            return result;
        }

        public async Task<ItemStatsResult> GetItemStatsAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            var combos = await _comboRepository.GetAllAsync();

            // Count order quantities per product and combo
            var productSales = orders
                .Where(o => o.Status == "Completed")
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.ProductId.HasValue)
                .GroupBy(oi => oi.ProductId!.Value)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.Quantity));

            var comboSales = orders
                .Where(o => o.Status == "Completed")
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.ComboId.HasValue)
                .GroupBy(oi => oi.ComboId!.Value)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.Quantity));

            var result = new ItemStatsResult
            {
                Products = products.Select(p => new ItemStatItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    TotalSold = productSales.ContainsKey(p.Id) ? productSales[p.Id] : 0,
                    IsActive = p.IsActive
                }).OrderByDescending(x => x.TotalSold).ToList(),

                Combos = combos.Select(c => new ItemStatItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    TotalSold = comboSales.ContainsKey(c.Id) ? comboSales[c.Id] : 0,
                    IsActive = c.IsActive
                }).OrderByDescending(x => x.TotalSold).ToList()
            };

            return result;
        }
    }
}
