using FastFood.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Order?> GetOrderWithDetailsAsync(int id);
        Task<Order?> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
    }
}
