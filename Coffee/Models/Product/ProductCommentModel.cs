using FastFood.Models.Comment;
using FastFood.Models.Common;

namespace FastFood.Models.Product
{
    public class ProductCommentModel
    {
        public ProductDetailModel Product { get; set; } = new();
        public List<CommentModel> Comments { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
    }
}
