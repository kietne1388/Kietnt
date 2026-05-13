using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class Combo
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal ComboPrice { get; set; }

        public string? ComboType { get; set; } // "1 Người", "2 Người", "3+ Người", "Gia Đình"
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
