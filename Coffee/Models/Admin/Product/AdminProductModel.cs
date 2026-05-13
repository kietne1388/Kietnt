using FastFood.Models.Product;
using FastFood.Models.Common;

namespace FastFood.Models.Admin.Product
{
    public class AdminProductModel
    {
        public List<ProductModel> Products { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
    }
}
