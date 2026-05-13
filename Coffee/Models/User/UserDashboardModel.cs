using FastFood.Models.Order;

namespace FastFood.Models.User
{
    public class UserDashboardModel
    {
        public UserProfileModel Profile { get; set; } = new();
        public List<OrderModel> RecentOrders { get; set; } = new();
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
