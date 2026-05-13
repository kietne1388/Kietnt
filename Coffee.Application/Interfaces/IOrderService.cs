using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(int userId, string address, List<CartItemDto> items, decimal discountAmount = 0);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(string? statusFilter = null);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
