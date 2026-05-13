using FastFood.Models.User;
using FastFood.Models.Common;

namespace FastFood.Models.Admin.User
{
    public class AdminUserModel
    {
        public List<UserProfileModel> Users { get; set; } = new();
        public PaginationModel Pagination { get; set; } = new();
    }
}
