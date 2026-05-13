using FastFood.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<(IEnumerable<FoodOrderItemDto> Items, int TotalCount)> GetPagedOrderItemsAsync(int page, int size);
        Task<FoodOrderItemDto?> GetByIdAsync(int id);
        Task<FoodOrderItemDto> CreateOrderItemAsync(int foodId, int quantity);
        Task<bool> DeleteOrderItemAsync(int id);
    }
}
