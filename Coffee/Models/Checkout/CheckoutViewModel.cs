using FastFood.Models.Cart;

namespace FastFood.Models.Checkout
{
    public class CheckoutViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; } = 20000;
        public decimal Discount { get; set; } = 0;
        public decimal Total => SubTotal + ShippingFee - Discount;

        // Customer info
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string? Note { get; set; }
        public string PaymentMethod { get; set; } = "COD";
        public string? VoucherCode { get; set; }
    }
}
