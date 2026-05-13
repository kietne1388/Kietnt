using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq; // Thêm Linq để dùng Select nếu cần
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces; // Quan trọng: Để dùng IUserRepository

namespace FastFood.Application.Services
{
    public class AuthService : IAuthService
    {
        // 👇 SỬA Ở ĐÂY: Dùng Interface thay vì Class
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        // 👇 SỬA CONSTRUCTOR: Nhận vào các Repository cần thiết
        public AuthService(IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<UserDto?> RegisterAsync(string username, string password, string fullName, string email, string phoneNumber)
        {
            if (await _userRepository.IsUsernameExistsAsync(username)) return null;
            if (await _userRepository.IsEmailExistsAsync(email)) return null;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto?> LoginAsync(string usernameOrEmail, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(usernameOrEmail);
            
            // If empty by username, try email
            if (user == null && usernameOrEmail.Contains("@"))
            {
                user = await _userRepository.GetByEmailAsync(usernameOrEmail);
            }

            if (user == null || !user.IsActive) return null;

            try
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Start of a fix: if salt is invalid, the password cannot be verified. 
                // Treat as wrong password (or log it).
                return null;
            }

            user.LastLoginDate = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            return MapToDto(user);
        }

        public async Task<UserDto?> ExternalLoginAsync(string provider, string email, string fullName)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            
            if (user == null)
            {
                var username = email.Split('@')[0] + "_" + provider.ToLower();
                var randomPassword = Guid.NewGuid().ToString();
                
                user = new User
                {
                    Username = username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword),
                    FullName = fullName,
                    Email = email,
                    PhoneNumber = "",
                    Role = "User",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    LastLoginDate = DateTime.Now
                };

                await _userRepository.AddAsync(user);
            }
            else
            {
                user.LastLoginDate = DateTime.Now;
                await _userRepository.UpdateAsync(user);
            }

            return MapToDto(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var dto = MapToDto(user);
            
            // Fetch stats
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var completedOrders = orders.Where(o => o.Status == "Completed").ToList();
            
            dto.OrderCount = orders.Count();
            dto.TotalSpent = completedOrders.Sum(o => o.TotalAmount);
            
            return dto;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash)) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<bool> UpdateUserAsync(int userId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> UpdateMembershipTierAsync(int userId, string tier)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.MembershipTier = tier;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}