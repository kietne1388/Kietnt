using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IUserVoucherRepository
    {
        Task<IEnumerable<UserVoucher>> GetAllAsync();
        Task<UserVoucher?> GetByIdAsync(int id);
        Task AddAsync(UserVoucher userVoucher);
        Task UpdateAsync(UserVoucher userVoucher);
        Task DeleteAsync(UserVoucher userVoucher);

        Task<IEnumerable<UserVoucher>> GetUserVouchersAsync(int userId);
        Task<UserVoucher?> GetUserVoucherAsync(int userId, int voucherId);
        Task AddRangeAsync(IEnumerable<UserVoucher> userVouchers);
        Task<bool> HasUserReceivedDailyVoucherAsync(int userId, DateTime date);
    }
}
