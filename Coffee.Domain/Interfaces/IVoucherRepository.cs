using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher?> GetByIdAsync(int id);
        Task AddAsync(Voucher voucher);
        Task UpdateAsync(Voucher voucher);
        Task DeleteAsync(Voucher voucher);

        Task<Voucher?> GetByCodeAsync(string code);
        Task<IEnumerable<Voucher>> GetActiveVouchersAsync();
    }
}
