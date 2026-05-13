using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FastFood.Domain.Interfaces; // Added namespace

namespace FastFood.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Combo)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Combo)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // Alias for consistency with service expectations
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await GetOrdersByUserAsync(userId);
        }

        public async Task<Order?> GetOrderDetailAsync(int orderId)
        {
            return await _dbSet
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Combo)
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        // Alias for consistency with service expectations
        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await GetOrderDetailAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Combo)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
