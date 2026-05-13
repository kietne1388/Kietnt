namespace FastFood.Models.Voucher
{
    public class ApplyVoucherModel
    {
        public string VoucherCode { get; set; } = string.Empty;
        public decimal OrderAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
