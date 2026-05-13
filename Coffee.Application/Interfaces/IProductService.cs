using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(string? searchTerm = null, int? categoryId = null);
        Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetPagedProductsAsync(int page, int size);
        Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(string name, string description, decimal price, string imageUrl, int categoryId);
        Task<ProductDto?> UpdateProductAsync(int id, string name, string description, decimal price, string imageUrl, int categoryId);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
        Task<IEnumerable<ProductDto>> FilterByPriceAsync(decimal min, decimal max);
    }
}
