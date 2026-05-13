using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Entities; // Restored missing namespace
// using FastFood.Models.Cart; // Removed MVC dependency

namespace FastFood.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IComboRepository _comboRepository; 

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository, 
            IUserRepository userRepository,
            IComboRepository comboRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _comboRepository = comboRepository;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, string address, List<CartItemDto> items, decimal discountAmount = 0)
        {
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in items)
            {
                // TRƯỜNG HỢP 1: Mua Combo
                if (item.ComboId.HasValue)
                {
                    // Lấy giá chuẩn từ DB để bảo mật (tránh hack giá từ frontend)
                    var combo = await _comboRepository.GetByIdAsync(item.ComboId.Value);
                    if (combo == null || !combo.IsActive) continue;

                    var orderItem = new OrderItem
                    {
                        ProductId = null,        // Combo thì ProductId là null
                        ComboId = combo.Id,      // Lưu ID Combo
                        Quantity = item.Quantity,
                        UnitPrice = combo.ComboPrice // Lấy giá Combo
                    };
                    orderItems.Add(orderItem);
                    totalAmount += combo.ComboPrice * item.Quantity;
                }
                // TRƯỜNG HỢP 2: Mua Món lẻ (ProductId có value)
                else if (item.ProductId > 0) 
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null || !product.IsActive) continue;

                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        ComboId = null,          // Món lẻ thì ComboId là null
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };
                    orderItems.Add(orderItem);
                    totalAmount += product.Price * item.Quantity;
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderCode = GenerateOrderCode(),
                TotalAmount = totalAmount - discountAmount,
                Status = "Pending",
                Address = address,
                CreatedAt = DateTime.Now,
                OrderItems = orderItems
            };

            await _orderRepository.AddAsync(order);

            // Load user info để trả về DTO đầy đủ
            var user = await _userRepository.GetByIdAsync(userId);
            order.User = user!;

            return await MapToDto(order);
        }

        // --- CÁC HÀM GET GIỮ NGUYÊN LOGIC, CHỈ GỌI LẠI MAPDTO ---
        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var dtos = new List<OrderDto>();
            foreach (var order in orders) dtos.Add(await MapToDto(order));
            return dtos;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(string? statusFilter = null)
        {
            var orders = await _orderRepository.GetAllAsync();
            
            if (!string.IsNullOrEmpty(statusFilter))
            {
                orders = orders.Where(o => o.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var dtos = new List<OrderDto>();
            foreach (var order in orders) dtos.Add(await MapToDto(order));
            return dtos;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(id);
            return order == null ? null : await MapToDto(order);
        }

        // --- CÁC HÀM UPDATE STATUS GIỮ NGUYÊN ---
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;
            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.Status != "Pending") return false;
            order.Status = "Cancelled";
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        private string GenerateOrderCode()
        {
            return $"ORD{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        // 3. LOGIC QUAN TRỌNG: MAP TÊN SẢN PHẨM HOẶC COMBO
        private async Task<OrderDto> MapToDto(Order order)
        {
            var orderItemDtos = new List<OrderItemDto>();

            foreach (var item in order.OrderItems)
            {
                string itemName = "Không xác định";
                // Logic lấy tên món
                if (item.ProductId.HasValue)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId.Value);
                    itemName = product?.Name ?? "Sản phẩm đã xóa";
                }
                else if (item.ComboId.HasValue)
                {
                    var combo = await _comboRepository.GetByIdAsync(item.ComboId.Value);
                    itemName = combo?.Name ?? "Combo đã xóa";
                }

                orderItemDtos.Add(new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId ?? 0, 
                    ComboId = item.ComboId, // Added ComboId mapping
                    
                    ProductName = itemName, // Đã hiển thị đúng tên
                    Quantity = item.Quantity,
                    Price = item.UnitPrice,
                    Subtotal = item.UnitPrice * item.Quantity
                });
            }

            return new OrderDto
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                UserId = order.UserId,
                UserName = order.User?.FullName ?? "",
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Address = order.Address,
                CreatedAt = order.CreatedAt,
                OrderItems = orderItemDtos
            };
        }
    }
}