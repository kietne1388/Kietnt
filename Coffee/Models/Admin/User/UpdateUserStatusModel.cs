namespace FastFood.Models.Admin.User
{
    public class UpdateUserStatusModel
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
