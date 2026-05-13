namespace FastFood.Models.Comment
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5 stars
        public bool IsHidden { get; set; }
        public int? ParentId { get; set; }
        public List<CommentModel> Replies { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
