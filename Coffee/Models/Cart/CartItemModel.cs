namespace FastFood.Models.Cart
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public int? ComboId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Subtotal => Price * Quantity;
        public bool IsCombo => ComboId.HasValue && ComboId.Value > 0;
        public Dictionary<int, string> ComboItemNotes { get; set; } = new Dictionary<int, string>();
    }
}
