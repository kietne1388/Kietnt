using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FastFood.Domain.Interfaces;
namespace FastFood.Infrastructure.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(AppDbContext context) : base(context) { }

        public async Task<Voucher?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x =>
                    x.Code == code &&
                    x.IsActive &&
                    x.ExpiredAt > DateTime.Now &&
                    x.Quantity > 0);
        }

        public async Task<IEnumerable<Voucher>> GetActiveVouchersAsync()
        {
            return await _dbSet
                .Where(x => x.IsActive && x.ExpiredAt > DateTime.Now && x.Quantity > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetVouchersByTypeAsync(string type)
        {
            return await _dbSet
                .Where(x => x.IsActive && 
                            x.ExpiredAt > DateTime.Now && 
                            x.Quantity > 0 &&
                            x.Code.StartsWith(type))
                .ToListAsync();
        }
    }
}
