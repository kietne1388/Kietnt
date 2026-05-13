using System.Collections.Generic;
using System.Threading.Tasks;
using FastFood.Application.DTOs;

namespace FastFood.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<NotificationDto> CreateNotificationAsync(int? userId, string type, string title, string message, string? url = null);
        Task BroadcastNotificationAsync(string type, string title, string message, string? url = null);
    }
}
