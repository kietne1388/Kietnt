using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FastFood.Domain.Interfaces;

namespace FastFood.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet
                .Include(x => x.Products)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(x => x.IsActive)
                .Include(x => x.Products)
                .ToListAsync();
        }
    }
}
