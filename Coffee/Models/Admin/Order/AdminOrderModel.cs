using FastFood.Models.Order;
using FastFood.Models.Common;

namespace FastFood.Models.Admin.Order
{
    public class AdminOrderModel
    {
        public List<OrderModel> Orders { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
        public string? StatusFilter { get; set; }
    }
}
