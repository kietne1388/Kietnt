using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FastFood.Infrastructure.Context;

using FastFood.Domain.Interfaces; // Added namespace

namespace FastFood.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Include(x => x.Category)
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterByPriceAsync(decimal min, decimal max)
        {
            return await _dbSet
                .Where(x => x.Price >= min && x.Price <= max && x.IsActive)
                .ToListAsync();
        }
    }
}
