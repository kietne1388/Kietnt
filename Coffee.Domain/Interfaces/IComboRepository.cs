using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IComboRepository
    {
        // Các hàm cơ bản
        Task<IEnumerable<Combo>> GetAllAsync();
        Task<Combo?> GetByIdAsync(int id);
        Task AddAsync(Combo combo);
        Task UpdateAsync(Combo combo);
        Task DeleteAsync(Combo combo);

        // 👇 CÁC HÀM QUAN TRỌNG ĐỂ HIỂN THỊ TRANG CHỦ
        Task<IEnumerable<Combo>> GetActiveCombosAsync(); // Lấy combo đang hoạt động
        Task<Combo?> GetComboDetailAsync(int id); // Lấy chi tiết kèm món ăn
    }
}