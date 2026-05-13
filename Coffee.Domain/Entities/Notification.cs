using System;

namespace FastFood.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int? UserId { get; set; }  // null = notification for all users
        public string Type { get; set; } = null!;  // "NewProduct", "NewCombo", "NewVoucher", "VoucherExpiring"
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Url { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User? User { get; set; }
    }
}
