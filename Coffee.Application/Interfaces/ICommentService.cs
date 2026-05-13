using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByProductIdAsync(int productId);
        Task<IEnumerable<CommentDto>> GetCommentsByComboIdAsync(int comboId);
        Task<CommentDto> CreateCommentAsync(int userId, int? productId, int? comboId, string content, int rating, int? parentId = null);
        Task<bool> HideCommentAsync(int commentId);
        Task<bool> DeleteCommentAsync(int commentId, int requestingUserId);
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
    }
}
