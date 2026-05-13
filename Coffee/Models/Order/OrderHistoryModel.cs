using FastFood.Models.Common;

namespace FastFood.Models.Order
{
    public class OrderHistoryModel
    {
        public List<OrderModel> Orders { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
    }
}
