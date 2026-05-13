using System;
using System.Collections.Generic;
using System.Text;

using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Repositories;

namespace FastFood.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IUserVoucherRepository _userVoucherRepository;

        public VoucherService(IVoucherRepository voucherRepository, IUserVoucherRepository userVoucherRepository)
        {
            _voucherRepository = voucherRepository;
            _userVoucherRepository = userVoucherRepository;
        }

        public async Task<IEnumerable<VoucherDto>> GetAllVouchersAsync(string? searchTerm = null)
        {
            var vouchers = await _voucherRepository.GetAllAsync();

            var query = vouchers.AsEnumerable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                       (v.Description != null && v.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            return query.Select(MapToDto);
        }

        public async Task<IEnumerable<VoucherDto>> GetActiveVouchersAsync()
        {
            var vouchers = await _voucherRepository.GetActiveVouchersAsync();
            return vouchers.Select(MapToDto);
        }

        public async Task<VoucherDto?> GetVoucherByCodeAsync(string code)
        {
            var voucher = await _voucherRepository.GetByCodeAsync(code);
            return voucher == null ? null : MapToDto(voucher);
        }

        public async Task<VoucherDto> CreateVoucherAsync(string code, decimal discountAmount, int? discountPercent, DateTime expiredAt, int quantity)
        {
            var voucher = new Voucher
            {
                Code = code,
                DiscountAmount = discountAmount,
                DiscountPercent = discountPercent,
                ExpiredAt = expiredAt,
                Quantity = quantity,
                IsActive = true
            };

            await _voucherRepository.AddAsync(voucher);
            return MapToDto(voucher);
        }

        public async Task<bool> ApplyVoucherAsync(string code)
        {
            var voucher = await _voucherRepository.GetByCodeAsync(code);
            if (voucher == null || !voucher.IsActive || voucher.ExpiredAt < DateTime.Now || voucher.Quantity <= 0)
                return false;

            voucher.Quantity--;
            await _voucherRepository.UpdateAsync(voucher);
            return true;
        }

        public async Task<bool> DeleteVoucherAsync(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null) return false;
            await _voucherRepository.DeleteAsync(voucher);
            return true;
        }

        public async Task<decimal> CalculateDiscountAsync(string code, decimal orderAmount)
        {
            var voucher = await _voucherRepository.GetByCodeAsync(code);
            if (voucher == null || !voucher.IsActive || voucher.ExpiredAt < DateTime.Now || voucher.Quantity <= 0)
                return 0;

            if (voucher.DiscountPercent.HasValue)
            {
                return orderAmount * voucher.DiscountPercent.Value / 100;
            }

            return voucher.DiscountAmount;
        }

        // Assign 5 discount vouchers + 3 freeship vouchers on registration
        public async Task AssignWelcomeVouchersAsync(int userId)
        {
            var userVouchers = new List<UserVoucher>();
            var expiredAt = DateTime.Now.AddMonths(3);

            // 5 Discount vouchers (10%, 15%, 20%, 25%, 30%)
            var discountPercents = new[] { 10, 15, 20, 25, 30 };
            for (int i = 0; i < 5; i++)
            {
                var voucher = await CreateVoucherAsync(
                    $"WELCOME_DISCOUNT_{userId}_{i + 1}_{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}",
                    0,
                    discountPercents[i],
                    expiredAt,
                    1
                );

                userVouchers.Add(new UserVoucher
                {
                    UserId = userId,
                    VoucherId = voucher.Id,
                    IsUsed = false,
                    AssignedAt = DateTime.Now
                });
            }

            // 3 Freeship vouchers (15000, 20000, 30000 VND)
            var freeshipAmounts = new[] { 15000m, 20000m, 30000m };
            for (int i = 0; i < 3; i++)
            {
                var voucher = await CreateVoucherAsync(
                    $"WELCOME_FREESHIP_{userId}_{i + 1}_{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}",
                    freeshipAmounts[i],
                    null,
                    expiredAt,
                    1
                );

                userVouchers.Add(new UserVoucher
                {
                    UserId = userId,
                    VoucherId = voucher.Id,
                    IsUsed = false,
                    AssignedAt = DateTime.Now
                });
            }

            await _userVoucherRepository.AddRangeAsync(userVouchers);
        }

        // Assign 1 discount + 1 freeship voucher on daily login
        public async Task AssignDailyLoginVouchersAsync(int userId)
        {
            // Check if user already received today's vouchers
            if (await _userVoucherRepository.HasUserReceivedDailyVoucherAsync(userId, DateTime.Today))
                return;

            var userVouchers = new List<UserVoucher>();
            var expiredAt = DateTime.Now.AddMonths(1);

            // 1 Discount voucher (5% off)
            var discountVoucher = await CreateVoucherAsync(
                $"DAILY_DISCOUNT_{userId}_{DateTime.Now:yyyyMMdd}_{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}",
                0,
                5,
                expiredAt,
                1
            );

            userVouchers.Add(new UserVoucher
            {
                UserId = userId,
                VoucherId = discountVoucher.Id,
                IsUsed = false,
                AssignedAt = DateTime.Now
            });

            // 1 Freeship voucher (10000 VND)
            var freeshipVoucher = await CreateVoucherAsync(
                $"DAILY_FREESHIP_{userId}_{DateTime.Now:yyyyMMdd}_{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}",
                10000,
                null,
                expiredAt,
                1
            );

            userVouchers.Add(new UserVoucher
            {
                UserId = userId,
                VoucherId = freeshipVoucher.Id,
                IsUsed = false,
                AssignedAt = DateTime.Now
            });

            await _userVoucherRepository.AddRangeAsync(userVouchers);
        }

        public async Task<IEnumerable<UserVoucherDto>> GetUserVouchersAsync(int userId)
        {
            var userVouchers = await _userVoucherRepository.GetUserVouchersAsync(userId);
            return userVouchers.Select(uv => new UserVoucherDto
            {
                UserId = uv.UserId,
                VoucherId = uv.VoucherId,
                VoucherCode = uv.Voucher.Code,
                DiscountAmount = uv.Voucher.DiscountAmount,
                DiscountPercent = uv.Voucher.DiscountPercent,
                ExpiredAt = uv.Voucher.ExpiredAt,
                IsUsed = uv.IsUsed,
                AssignedAt = uv.AssignedAt,
                UsedAt = uv.UsedAt,
                VoucherType = uv.Voucher.Code.Contains("FREESHIP") ? "Freeship" : "Discount"
            });
        }

        private VoucherDto MapToDto(Voucher voucher)
        {
            return new VoucherDto
            {
                Id = voucher.Id,
                Code = voucher.Code,
                Description = voucher.Description,
                DiscountAmount = voucher.DiscountAmount,
                DiscountPercent = voucher.DiscountPercent,
                ExpiredAt = voucher.ExpiredAt,
                Quantity = voucher.Quantity,
                IsActive = voucher.IsActive
            };
        }
    }
}

