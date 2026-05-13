namespace FastFood.Models.Auth
{
    public class ExternalLoginModel
    {
        public string Provider { get; set; } = string.Empty; // Google, Facebook, etc.
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
