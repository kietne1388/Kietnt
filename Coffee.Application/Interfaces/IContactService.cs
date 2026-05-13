using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Domain.Entities;

namespace FastFood.Application.Interfaces
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllContactsAsync();
        Task<Contact?> GetContactByIdAsync(int id);
        Task<Contact> CreateContactAsync(string name, string email, string subject, string message);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> DeleteContactAsync(int id);
    }
}
