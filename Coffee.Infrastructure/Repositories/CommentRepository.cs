using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FastFood.Domain.Interfaces;

namespace FastFood.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            return await _dbSet
                .Include(x => x.User)
                .Include(x => x.Replies.Where(r => !r.IsHidden))
                    .ThenInclude(r => r.User)
                .Where(x => x.ProductId == productId && !x.IsHidden && x.ParentId == null)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByComboIdAsync(int comboId)
        {
            return await _dbSet
                .Include(x => x.User)
                .Include(x => x.Replies.Where(r => !r.IsHidden))
                    .ThenInclude(r => r.User)
                .Where(x => x.ComboId == comboId && !x.IsHidden && x.ParentId == null)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetVisibleCommentsAsync()
        {
            return await _dbSet
                .Include(x => x.User)
                .Include(x => x.Product)
                .Where(x => !x.IsHidden)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingByProductIdAsync(int productId)
        {
            var comments = await _dbSet
                .Where(x => x.ProductId == productId && !x.IsHidden)
                .ToListAsync();

            if (!comments.Any())
                return 0;

            return comments.Average(x => x.Rating);
        }

        public async Task<double> GetAverageRatingByComboIdAsync(int comboId)
        {
            var comments = await _dbSet
                .Where(x => x.ComboId == comboId && !x.IsHidden)
                .ToListAsync();

            if (!comments.Any())
                return 0;

            return comments.Average(x => x.Rating);
        }
    }
}
