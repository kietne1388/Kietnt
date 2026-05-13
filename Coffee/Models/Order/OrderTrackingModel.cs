namespace FastFood.Models.Order
{
    public class OrderTrackingModel
    {
        public string OrderCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<TrackingStep> Steps { get; set; } = new();
    }

    public class TrackingStep
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted => CompletedAt.HasValue;
    }
}
