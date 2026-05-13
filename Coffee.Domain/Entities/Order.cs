using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string OrderCode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
