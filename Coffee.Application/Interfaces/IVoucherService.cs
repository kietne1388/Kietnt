using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;

namespace FastFood.Application.Interfaces
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherDto>> GetAllVouchersAsync(string? searchTerm = null);
        Task<IEnumerable<VoucherDto>> GetActiveVouchersAsync();
        Task<VoucherDto?> GetVoucherByCodeAsync(string code);
        Task<VoucherDto> CreateVoucherAsync(string code, decimal discountAmount, int? discountPercent, DateTime expiredAt, int quantity);
        Task<bool> ApplyVoucherAsync(string code);
        Task<bool> DeleteVoucherAsync(int id);
        Task<decimal> CalculateDiscountAsync(string code, decimal orderAmount);
        
        // New methods for user voucher assignment
        Task AssignWelcomeVouchersAsync(int userId);
        Task AssignDailyLoginVouchersAsync(int userId);
        Task<IEnumerable<UserVoucherDto>> GetUserVouchersAsync(int userId);
    }
}
