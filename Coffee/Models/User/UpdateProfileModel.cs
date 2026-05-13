using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.User
{
    public class UpdateProfileModel
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Avatar { get; set; }
        public string? Address { get; set; }
    }
}
