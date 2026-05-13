using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly AppDbContext _context;

        public OrderItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<FoodOrderItemDto> Items, int TotalCount)> GetPagedOrderItemsAsync(int page, int size)
        {
            var totalCount = await _context.OrderItems.CountAsync();
            var items = await _context.OrderItems
                .Include(x => x.Product)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x => MapToDto(x))
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<FoodOrderItemDto?> GetByIdAsync(int id)
        {
            var item = await _context.OrderItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<FoodOrderItemDto> CreateOrderItemAsync(int foodId, int quantity)
        {
            var product = await _context.Products.FindAsync(foodId);
            if (product == null)
                throw new ArgumentException("Food not found");

            var orderItem = new OrderItem
            {
                ProductId = foodId,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            // We need an Order to attach to - find or create a default one
            var defaultOrder = await _context.Orders.FirstOrDefaultAsync();
            if (defaultOrder == null)
            {
                defaultOrder = new Order
                {
                    UserId = 1,
                    TotalAmount = 0,
                    Status = "Pending",
                    OrderCode = "DEFAULT-" + DateTime.Now.Ticks,
                    Address = "N/A"
                };
                _context.Orders.Add(defaultOrder);
                await _context.SaveChangesAsync();
            }

            orderItem.OrderId = defaultOrder.Id;
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            // Reload with product
            var saved = await _context.OrderItems
                .Include(x => x.Product)
                .FirstAsync(x => x.Id == orderItem.Id);

            return MapToDto(saved);
        }

        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return false;

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        private static FoodOrderItemDto MapToDto(OrderItem item)
        {
            return new FoodOrderItemDto
            {
                Id = item.Id,
                FoodId = item.ProductId,
                FoodName = item.Product?.Name ?? "",
                Quantity = item.Quantity,
                PriceAtOrder = item.UnitPrice,
                OrderDate = DateTime.Now
            };
        }
    }
}
