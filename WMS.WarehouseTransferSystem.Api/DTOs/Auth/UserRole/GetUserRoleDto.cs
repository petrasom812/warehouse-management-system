namespace WMS.WarehouseTransferSystem.Api.DTOs.Auth
{
    public class GetUserRoleDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}