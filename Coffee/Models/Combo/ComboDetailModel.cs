using Microsoft.AspNetCore.Http;

namespace FastFood.Models.Combo
{
    public class ComboDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ComboPrice { get; set; }
        public string? ComboType { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SavedAmount { get; set; }
        public decimal Price => ComboPrice; // Alias for ComboPrice
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<ComboItemModel> Items { get; set; } = new();
        public List<FastFood.Application.DTOs.CommentDto> Comments { get; set; } = new();
        public int CommentCount { get; set; }
        public double AverageRating { get; set; }
    }
}
