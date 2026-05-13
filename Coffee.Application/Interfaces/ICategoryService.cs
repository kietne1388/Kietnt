using FastFood.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<(IEnumerable<CategoryDto> Items, int TotalCount)> GetPagedCategoriesAsync(int page, int size);
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<bool> CreateCategoryAsync(string name, string? description);
        Task<bool> UpdateCategoryAsync(int id, string name, string? description);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
