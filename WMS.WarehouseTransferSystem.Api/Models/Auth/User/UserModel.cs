using WMS.WarehouseTransferSystem.Api.Models.Auth.UserRole;

namespace WMS.WarehouseTransferSystem.Api.Models.Auth.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserRoleModel> UserRole {get; set;} = new List<UserRoleModel>();
    }
}