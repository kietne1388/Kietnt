using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Comment
{
    public class UpdateCommentModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
