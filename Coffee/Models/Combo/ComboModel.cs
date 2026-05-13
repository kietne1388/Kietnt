using Microsoft.AspNetCore.Http;

namespace FastFood.Models.Combo
{
    public class ComboModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ComboPrice { get; set; }
        public string? ComboType { get; set; }
        public decimal SavedAmount => OriginalPrice - ComboPrice;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ComboItemModel> Items { get; set; } = new();
    }
}
