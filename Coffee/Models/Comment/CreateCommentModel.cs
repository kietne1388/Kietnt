using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Comment
{
    public class CreateCommentModel
    {
        public int? ProductId { get; set; }
        public int? ComboId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        public int? ParentId { get; set; }
    }
}
