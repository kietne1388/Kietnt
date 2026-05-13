using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Application.DTOs
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
    }
}
