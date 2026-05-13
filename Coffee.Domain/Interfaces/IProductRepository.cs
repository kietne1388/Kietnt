using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);

        // Các hàm lọc sản phẩm (để Product Service dùng)
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Product>> FilterByPriceAsync(decimal min, decimal max);
    }
}