using Microsoft.AspNetCore.Mvc;
using FastFood.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FastFood.Domain.Entities;

namespace FastFood.API.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId || n.UserId == null)
                .OrderByDescending(n => n.CreatedAt)
                .Take(20)
                .Select(n => new
                {
                    n.Id,
                    n.Type,
                    n.Title,
                    n.Message,
                    n.Url,
                    n.IsRead,
                    CreatedAt = n.CreatedAt.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpGet("unread-count/{userId}")]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var count = await _context.Notifications
                .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
                .CountAsync();

            return Ok(new { count });
        }

        [HttpPost("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound();

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Marked as read" });
        }

        [HttpPost("mark-all-read/{userId}")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "All notifications marked as read" });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Type = request.Type,
                Title = request.Title,
                Message = request.Message,
                Url = request.Url,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return Ok(notification);
        }
    }

    public record CreateNotificationRequest(int? UserId, string Type, string Title, string Message, string? Url);
}
