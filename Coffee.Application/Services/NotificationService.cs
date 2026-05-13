using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFood.Application.DTOs;
using FastFood.Application.Interfaces;
using FastFood.Domain.Entities;
using FastFood.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly GenericRepository<Notification> _notificationRepository;

        public NotificationService(GenericRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications
                .Where(n => n.UserId == userId || n.UserId == null)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Type = n.Type,
                    Title = n.Title,
                    Message = n.Message,
                    Url = n.Url,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                });
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null) return false;

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        public async Task<NotificationDto> CreateNotificationAsync(int? userId, string type, string title, string message, string? url = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                Url = url,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            await _notificationRepository.AddAsync(notification);

            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                Url = notification.Url,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }

        public async Task BroadcastNotificationAsync(string type, string title, string message, string? url = null)
        {
            await CreateNotificationAsync(null, type, title, message, url);
        }
    }
}
