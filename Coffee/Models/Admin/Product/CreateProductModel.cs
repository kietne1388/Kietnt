using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FastFood.Models.Admin.Product
{
    public class CreateProductModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        
        public IFormFile? ImageFile { get; set; }

        [Required]
        public int CategoryId { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
