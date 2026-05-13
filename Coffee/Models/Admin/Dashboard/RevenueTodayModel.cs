namespace FastFood.Models.Admin.Dashboard
{
    public class RevenueTodayModel
    {
        public DateTime Date { get; set; } = DateTime.Today;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}
