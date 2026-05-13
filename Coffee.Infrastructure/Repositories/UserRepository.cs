using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

using FastFood.Domain.Interfaces; // Added namespace

namespace FastFood.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithOrdersAsync(int userId)
        {
            return await _dbSet
                .Include(x => x.Orders)
                .ThenInclude(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.PhoneNumber == phone);
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(x => x.Email == email);
        }
    }
}
