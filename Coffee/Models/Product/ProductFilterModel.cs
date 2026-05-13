namespace FastFood.Models.Product
{
    public class ProductFilterModel
    {
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SearchKeyword { get; set; }
        public string? SortBy { get; set; } // price-asc, price-desc, name, rating
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
