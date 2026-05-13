using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";
        public string MembershipTier { get; set; } = "Normal"; // VIP, Loyal, Normal
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }

        // Employee specific fields (Persistent)
        public string? Position { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
    }

}
