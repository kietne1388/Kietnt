using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; // Thêm Linq
using System.Threading.Tasks; // Thêm Task
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces; // Dùng Interface Repository
using FastFood.Infrastructure.Repositories; // Dùng Class Repository (Cho Category tạm thời)

namespace FastFood.Application.Services
{
    public class ProductService : IProductService
    {
        // 👇 SỬA: Dùng IProductRepository
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository; 

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(string? searchTerm = null, int? categoryId = null)
        {
            var products = await _productRepository.GetAllAsync();

            var query = products.AsEnumerable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                       (p.Description != null && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return query.Select(MapToDto);
        }

        public async Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetPagedProductsAsync(int page, int size)
        {
            var allProducts = await _productRepository.GetAllAsync();
            var list = allProducts.ToList();
            var totalCount = list.Count;
            var items = list
                .Skip((page - 1) * size)
                .Take(size)
                .Select(MapToDto)
                .ToList();
            return (items, totalCount);
        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<ProductDto> CreateProductAsync(string name, string description, decimal price, string imageUrl, int categoryId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                IsActive = true
            };

            await _productRepository.AddAsync(product);

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            product.Category = category!;

            return MapToDto(product);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, string name, string description, decimal price, string imageUrl, int categoryId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            product.Name = name;
            product.Description = description;
            product.Price = price;
            product.ImageUrl = imageUrl;
            product.CategoryId = categoryId;

            await _productRepository.UpdateAsync(product);

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            product.Category = category!;

            return MapToDto(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            product.IsActive = !product.IsActive;
            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> FilterByPriceAsync(decimal min, decimal max)
        {
            var products = await _productRepository.FilterByPriceAsync(min, max);
            return products.Select(MapToDto);
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "",
                Comments = product.Comments?
                    .Where(c => !c.IsHidden && c.ParentId == null)
                    .Select(c => MapCommentToDto(c))
                    .ToList() ?? new List<CommentDto>()
            };
        }

        private CommentDto MapCommentToDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                UserName = comment.User?.FullName ?? "User",
                ProductId = comment.ProductId,
                ProductName = comment.Product?.Name ?? "",
                Content = comment.Content,
                Rating = comment.Rating,
                IsHidden = comment.IsHidden,
                CreatedAt = comment.CreatedAt,
                ParentId = comment.ParentId,
                Replies = comment.Replies?
                    .Where(r => !r.IsHidden)
                    .Select(MapCommentToDto)
                    .ToList() ?? new List<CommentDto>()
            };
        }
    }
}