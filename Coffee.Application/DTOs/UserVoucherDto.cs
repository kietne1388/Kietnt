using System;

namespace FastFood.Application.Interfaces
{
    public class UserVoucherDto
    {
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public string VoucherType { get; set; } = string.Empty; // "Discount" or "Freeship"
    }
}
