namespace FastFood.Models.Admin.Order
{
    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
