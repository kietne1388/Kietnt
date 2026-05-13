using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
}
