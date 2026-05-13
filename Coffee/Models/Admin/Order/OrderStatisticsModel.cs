namespace FastFood.Models.Admin.Order
{
    public class OrderStatisticsModel
    {
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int ShippingCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
    }
}
