namespace FastFood.Models.Order
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public string? Note => Notes; // Alias for view compatibility
        public List<OrderItemModel> Items { get; set; } = new();
    }
}
