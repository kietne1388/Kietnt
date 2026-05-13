using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.Auth
{
    public class VerifyOtpModel
    {
        [Required]
        public string PhoneNumberOrEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OtpCode { get; set; } = string.Empty;
    }
}
