using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.WarehouseTransferSystem.Api.Models.Auth.Role;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;

namespace WMS.WarehouseTransferSystem.Api.Models.Auth.UserRole
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public UserModel User { get; set; } = null!;
        public RoleModel Role { get; set; } = null!;
    }
}