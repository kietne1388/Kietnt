using FastFood.Models.Comment;

namespace FastFood.Models.Product
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public double AverageRating { get; set; }
        public int CommentCount { get; set; }
        public List<CommentModel> Comments { get; set; } = new();
        public List<ProductModel> RelatedProducts { get; set; } = new();
    }
}
