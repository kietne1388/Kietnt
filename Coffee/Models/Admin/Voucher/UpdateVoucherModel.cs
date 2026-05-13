using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Admin.Voucher
{
    public class UpdateVoucherModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal DiscountAmount { get; set; }
        public int? DiscountPercent { get; set; }

        [Required]
        public DateTime ExpiredAt { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
