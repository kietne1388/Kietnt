using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class Voucher
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiredAt { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
    }
}
