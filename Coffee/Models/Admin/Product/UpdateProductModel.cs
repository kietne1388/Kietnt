using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Admin.Product
{
    public class UpdateProductModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        
        public string? CurrentImageUrl => ImageUrl; // Alias for view compatibility

        [Required]
        public int CategoryId { get; set; }
        
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; }
    }
}
