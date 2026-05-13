using System;

namespace FastFood.Application.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int? ComboId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
