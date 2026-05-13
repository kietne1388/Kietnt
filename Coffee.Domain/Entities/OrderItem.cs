using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // --- SỬA Ở ĐÂY ---

        // 1. Đổi thành int? (Nullable) vì nếu khách mua Combo thì ProductId sẽ bằng null
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        // 2. Thêm ComboId (cũng là Nullable) để lưu nếu khách mua Combo
        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }

        // -----------------

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}