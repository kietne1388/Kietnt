namespace FastFood.Models.Cart
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
        public bool IsSelected { get; set; } = true;

        // --- BỔ SUNG PHẦN NÀY ---
        public int? ComboId { get; set; } // Để lưu ID của Combo (nếu có)
        public bool IsCombo => ComboId.HasValue && ComboId.Value > 0; // Check nhanh

        // Dictionary để lưu ghi chú cho từng món trong Combo
        // Key: ProductId, Value: Note
        public Dictionary<int, string> ComboItemNotes { get; set; } = new Dictionary<int, string>();
    }
}