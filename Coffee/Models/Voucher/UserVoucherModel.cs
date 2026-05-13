namespace FastFood.Models.Voucher
{
    public class UserVoucherModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
    }
}
