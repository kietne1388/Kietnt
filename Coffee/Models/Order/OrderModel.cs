namespace FastFood.Models.Order
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Shipping, Completed, Cancelled
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemModel>? Items { get; set; }
    }

    public class OrderItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
