using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);

        Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId);
        Task<IEnumerable<Comment>> GetCommentsByComboIdAsync(int comboId);
        Task<IEnumerable<Comment>> GetVisibleCommentsAsync();
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
        Task<double> GetAverageRatingByProductIdAsync(int productId);
        Task<double> GetAverageRatingByComboIdAsync(int comboId);
    }
}
