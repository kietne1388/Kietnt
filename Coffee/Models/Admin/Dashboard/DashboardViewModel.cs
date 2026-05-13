namespace FastFood.Models.Admin.Dashboard
{
    public class DashboardViewModel
    {
        public RevenueTodayModel Today { get; set; } = new();
        public RevenueMonthModel Month { get; set; } = new();
        public RevenueYearModel Year { get; set; } = new();
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public List<RecentOrderModel> RecentOrders { get; set; } = new();
        public List<TopProductModel> TopProducts { get; set; } = new();
    }

    public class RecentOrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = "";
        public string UserName { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public class TopProductModel
    {
        public string ProductName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
    }
}
