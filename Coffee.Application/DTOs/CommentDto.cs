using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? ComboId { get; set; }
        public string? ComboName { get; set; }
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsHidden { get; set; }
        public int? ParentId { get; set; }
        public List<CommentDto> Replies { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
