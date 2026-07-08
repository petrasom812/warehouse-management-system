using WMS.WarehouseTransferSystem.Api.DTOs.Auth;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Auth
{
    public interface IServiceUserRole
    {
        Task<GetUserRoleDto> CreateUserRolelAsync(CreateUserRoleDto dto);
        Task<List<GetUserRoleDto>> GetUserRoleAsync();
        Task<GetUserRoleDto?> GetUserRoleByIdAsync(int id);
    }
}