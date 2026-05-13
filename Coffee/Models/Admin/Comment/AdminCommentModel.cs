using FastFood.Models.Comment;
using FastFood.Models.Common;

namespace FastFood.Models.Admin.Comment
{
    public class AdminCommentModel
    {
        public List<CommentModel> Comments { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
    }
}
