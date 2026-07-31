

using WMS.WarehouseTransferSystem.Api.Models.Auth.UserRole;

namespace WMS.WarehouseTransferSystem.Api.Models.Auth.Role
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public ICollection<UserRoleModel> UserRole {get; set;} = new List<UserRoleModel>();
    }
}