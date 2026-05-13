using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FastFood.Domain.Interfaces;
namespace FastFood.Infrastructure.Repositories
{
    public class UserVoucherRepository : GenericRepository<UserVoucher>, IUserVoucherRepository
    {
        public UserVoucherRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<UserVoucher>> GetUserVouchersAsync(int userId)
        {
            return await _dbSet
                .Include(uv => uv.Voucher)
                .Where(uv => uv.UserId == userId && !uv.IsUsed)
                .OrderByDescending(uv => uv.AssignedAt)
                .ToListAsync();
        }

        public async Task<UserVoucher?> GetUserVoucherAsync(int userId, int voucherId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(uv => uv.UserId == userId && uv.VoucherId == voucherId);
        }

        public override async Task AddAsync(UserVoucher userVoucher)
        {
            await _dbSet.AddAsync(userVoucher);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<UserVoucher> userVouchers)
        {
            await _dbSet.AddRangeAsync(userVouchers);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserReceivedDailyVoucherAsync(int userId, DateTime date)
        {
            return await _dbSet
                .AnyAsync(uv => uv.UserId == userId && 
                               uv.AssignedAt.Date == date.Date);
        }
    }
}
