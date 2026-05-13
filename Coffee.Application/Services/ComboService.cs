using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Entities;

namespace FastFood.Application.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepository;
        private readonly IProductRepository _productRepository;

        public ComboService(IComboRepository comboRepository, IProductRepository productRepository)
        {
            _comboRepository = comboRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ComboDto>> GetAllCombosAsync(string? searchTerm = null)
        {
            var combos = await _comboRepository.GetAllAsync();

            var query = combos.AsEnumerable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                       (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            return query.Select(MapToDto);
        }

        public async Task<IEnumerable<ComboDto>> GetActiveCombosAsync()
        {
            // Hàm này lấy Combo + Product con
            var combos = await _comboRepository.GetActiveCombosAsync();
            return combos.Select(MapToDto);
        }

        public async Task<ComboDto?> GetComboByIdAsync(int id)
        {
            var combo = await _comboRepository.GetComboDetailAsync(id);
            return combo == null ? null : MapToDto(combo);
        }

        public async Task<ComboDto> CreateComboAsync(string name, string? description, decimal comboPrice, string? comboType, string? imageUrl, List<(int ProductId, int Quantity)> items)
        {
            var comboItems = new List<ComboItem>();
            decimal originalPrice = 0;

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                comboItems.Add(new ComboItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });

                originalPrice += product.Price * item.Quantity;
            }

            var combo = new Combo
            {
                Name = name,
                Description = description,
                OriginalPrice = originalPrice,
                ComboPrice = comboPrice,
                ComboType = comboType,
                ImageUrl = imageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ComboItems = comboItems
            };

            await _comboRepository.AddAsync(combo);
            return MapToDto(combo);
        }

        public async Task<bool> UpdateComboAsync(int id, string name, string? description, decimal comboPrice, string? comboType, string? imageUrl, List<(int ProductId, int Quantity)> items)
        {
            var combo = await _comboRepository.GetComboDetailAsync(id);
            if (combo == null) return false;

            combo.Name = name;
            combo.Description = description;
            combo.ComboPrice = comboPrice;
            combo.ComboType = comboType;
            combo.ImageUrl = imageUrl;
            combo.UpdatedAt = DateTime.UtcNow;

            // Update items
            var updatedComboItems = new List<ComboItem>();
            decimal originalPrice = 0;

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                updatedComboItems.Add(new ComboItem
                {
                    ComboId = id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });

                originalPrice += product.Price * item.Quantity;
            }

            combo.OriginalPrice = originalPrice;
            
            // Clear and replace items to ensure sync
            if (combo.ComboItems != null)
            {
                combo.ComboItems.Clear();
                foreach (var item in updatedComboItems)
                {
                    combo.ComboItems.Add(item);
                }
            }
            else
            {
                combo.ComboItems = updatedComboItems;
            }

            await _comboRepository.UpdateAsync(combo);
            return true;
        }
        public async Task<bool> DeleteComboAsync(int id)
        {
            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) return false;

            await _comboRepository.DeleteAsync(combo);
            return true;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) return false;

            combo.IsActive = !combo.IsActive;
            combo.UpdatedAt = DateTime.UtcNow;
            await _comboRepository.UpdateAsync(combo);
            return true;
        }
        // 👇 HÀM MAP ĐÃ SỬA LỖI (Quan trọng nhất)
        private ComboDto MapToDto(Combo combo)
        {
            return new ComboDto
            {
                Id = combo.Id,
                Name = combo.Name,
                Description = combo.Description,
                OriginalPrice = combo.OriginalPrice,
                ComboPrice = combo.ComboPrice,
                ComboType = combo.ComboType,
                ImageUrl = combo.ImageUrl,
                IsActive = combo.IsActive,
                CreatedAt = combo.CreatedAt,
                
                // Map danh sách món ăn
                ComboItems = combo.ComboItems?.Select(item => new ComboItemDto
                {
                    // Đã xóa dòng Id = item.Id vì bảng không có cột này (Sửa lỗi CS1061)
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    
                    // Map thông tin sản phẩm con để hiển thị ảnh/tên
                    Product = new ProductDto 
                    {
                        Id = item.Product?.Id ?? 0,
                        Name = item.Product?.Name ?? "Loading...",
                        ImageUrl = item.Product?.ImageUrl ?? "/images/default.png", 
                        Price = item.Product?.Price ?? 0,
                        Description = item.Product?.Description ?? ""
                    }
                }).ToList() ?? new List<ComboItemDto>()
            };
        }
    }
}