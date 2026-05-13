using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Statistics
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int UsedVoucherCount { get; set; }
    }
}
