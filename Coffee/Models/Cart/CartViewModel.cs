namespace FastFood.Models.Cart
{
    public class CartViewModel
    {
        public List<CartItemModel> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public int TotalItems => Items.Sum(i => i.Quantity);
        public string? VoucherCode { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal FinalAmount => TotalAmount - (DiscountAmount ?? 0);
    }
}
