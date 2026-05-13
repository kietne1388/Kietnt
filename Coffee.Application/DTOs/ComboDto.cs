using System;
using System.Collections.Generic;

namespace FastFood.Application.DTOs
{
    public class ComboDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ComboPrice { get; set; }
        public decimal SavedAmount => OriginalPrice - ComboPrice;
        public string? ComboType { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public List<ComboItemDto> ComboItems { get; set; } = new();
    }

    public class ComboItemDto
    {
        // Đã xóa Id vì bảng ComboItem không có cột này
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // 👇 QUAN TRỌNG: Biến này để chứa thông tin món ăn (Tên, Ảnh)
        // Đây là cái bạn đang thiếu gây ra lỗi đỏ "does not contain definition for Product"
        public ProductDto Product { get; set; } = null!; 
    }
}