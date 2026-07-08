using System.ComponentModel.DataAnnotations;

namespace WMS.WarehouseTransferSystem.Api.DTOs.Auth.Role
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string RoleName { get; set; } = string.Empty;
    }
}