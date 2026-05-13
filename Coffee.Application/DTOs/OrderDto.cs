using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? ComboId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
