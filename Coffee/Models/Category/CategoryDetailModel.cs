using FastFood.Models.Product;

namespace FastFood.Models.Category
{
    public class CategoryDetailModel
    {
        public CategoryModel Category { get; set; } = new();
        public List<ProductModel> Products { get; set; } = new();
    }
}
