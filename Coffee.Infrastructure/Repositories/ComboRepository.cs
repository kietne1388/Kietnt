using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFood.Infrastructure.Repositories
{
    public class ComboRepository : IComboRepository
    {
        private readonly AppDbContext _context;

        public ComboRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Combo>> GetAllAsync()
        {
            return await _context.Combos.ToListAsync();
        }

        public async Task<Combo?> GetByIdAsync(int id)
        {
            return await _context.Combos.FindAsync(id);
        }

        public async Task AddAsync(Combo combo)
        {
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Combo combo)
        {
            _context.Combos.Update(combo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Combo combo)
        {
            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();
        }

        // 👇 HÀM QUAN TRỌNG: Lấy Combo hiển thị ra web
        public async Task<IEnumerable<Combo>> GetActiveCombosAsync()
        {
            return await _context.Combos
                .Where(c => c.IsActive) // Chỉ lấy cái đang bật
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        // 👇 HÀM CHI TIẾT: Lấy kèm danh sách món ăn bên trong
        public async Task<Combo?> GetComboDetailAsync(int id)
        {
            return await _context.Combos
                .Include(c => c.ComboItems) // Kèm chi tiết combo
                    .ThenInclude(ci => ci.Product) // Kèm thông tin món ăn (Tên, Ảnh)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}