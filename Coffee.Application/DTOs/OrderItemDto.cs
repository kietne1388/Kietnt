using System;

namespace FastFood.Application.DTOs
{
    public class FoodOrderItemDto
    {
        public int Id { get; set; }
        public int? FoodId { get; set; }
        public string FoodName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
