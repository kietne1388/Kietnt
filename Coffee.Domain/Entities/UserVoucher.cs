using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Domain.Entities
{
    public class UserVoucher
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; } = null!;

        public bool IsUsed { get; set; } = false;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UsedAt { get; set; }
    }

}
