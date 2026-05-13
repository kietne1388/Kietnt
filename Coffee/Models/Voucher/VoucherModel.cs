namespace FastFood.Models.Voucher
{
    public class VoucherModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public int? DiscountPercent { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired => DateTime.Now > ExpiredAt;
    }
}
