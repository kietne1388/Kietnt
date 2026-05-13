using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }

        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsHidden { get; set; } = false;
        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
