using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<CommentDto> Comments { get; set; } = new();
    }
}
