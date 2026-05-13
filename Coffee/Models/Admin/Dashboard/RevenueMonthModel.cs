namespace FastFood.Models.Admin.Dashboard
{
    public class RevenueMonthModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}
