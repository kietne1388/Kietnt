using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface IComboService
    {
        Task<IEnumerable<ComboDto>> GetAllCombosAsync(string? searchTerm = null);
        Task<IEnumerable<ComboDto>> GetActiveCombosAsync();
        Task<ComboDto?> GetComboByIdAsync(int id);
        Task<ComboDto> CreateComboAsync(string name, string? description, decimal comboPrice, string? comboType, string? imageUrl, List<(int ProductId, int Quantity)> items);
        Task<bool> UpdateComboAsync(int id, string name, string? description, decimal comboPrice, string? comboType, string? imageUrl, List<(int ProductId, int Quantity)> items);
        Task<bool> DeleteComboAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
    }
}
