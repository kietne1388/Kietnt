using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToDto);
        }

        public async Task<(IEnumerable<CategoryDto> Items, int TotalCount)> GetPagedCategoriesAsync(int page, int size)
        {
            var allCategories = await _categoryRepository.GetAllAsync();
            var list = allCategories.ToList();
            var totalCount = list.Count;
            var items = list
                .Skip((page - 1) * size)
                .Take(size)
                .Select(MapToDto)
                .ToList();
            return (items, totalCount);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : MapToDto(category);
        }

        public async Task<bool> CreateCategoryAsync(string name, string? description)
        {
            var category = new Category
            {
                Name = name,
                Description = description
            };
            await _categoryRepository.AddAsync(category);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(int id, string name, string? description)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            category.Name = name;
            category.Description = description;
            await _categoryRepository.UpdateAsync(category);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(category);
            return true;
        }

        private CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedAt,
                IsActive = category.IsActive,
                ProductCount = category.Products?.Count ?? 0
            };
        }
    }
}
