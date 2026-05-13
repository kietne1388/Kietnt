using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IUserRepository
    {
        // Các hàm CRUD cơ bản
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);

        // Các hàm kiểm tra (để Auth Service dùng)
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsEmailExistsAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPhoneAsync(string phone);
    }
}