using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Category
{
    public class CreateCategoryModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
