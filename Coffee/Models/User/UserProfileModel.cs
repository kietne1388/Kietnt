namespace FastFood.Models.User
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Phone => PhoneNumber; // Alias for view compatibility
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string Role { get; set; } = "Customer";
        public string MembershipTier { get; set; } = "Normal";
        public DateTime CreatedAt { get; set; }
        
        // Stats
        public int TotalOrders { get; set; }
        public int VoucherCount { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; } = true;

        // View compatibility properties
        public bool IsAdmin => Role == "Admin" || Role == "SuperAdmin";
        public int OrderCount => TotalOrders;
        public decimal TotalSpent { get; set; }
        public int VoucherUsedCount { get; set; }
    }
}
