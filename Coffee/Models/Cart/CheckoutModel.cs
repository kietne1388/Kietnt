using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Cart
{
    public class CheckoutModel
    {
        public List<CartItemModel> Items { get; set; } = new();

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        public string? VoucherCode { get; set; }
        public string? Notes { get; set; }
    }
}
