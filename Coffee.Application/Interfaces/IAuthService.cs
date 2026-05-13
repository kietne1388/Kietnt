using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(string username, string password, string fullName, string email, string phoneNumber);
        Task<UserDto?> LoginAsync(string usernameOrEmail, string password);
        Task<UserDto?> ExternalLoginAsync(string provider, string email, string fullName);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(int userId, bool isActive);
        Task<bool> UpdateMembershipTierAsync(int userId, string tier);
        string HashPassword(string password);
    }
}
