
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Auth
{
    public interface IServiceUser
    {
        Task<GetUserDto> CreateUserAsync(CreateUserDto dto);
        Task<List<GetUserDto>> GetUserAsync();
        Task<GetUserDto?> GetUserByIdAsync(int id);
        Task<GetUserDto?> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> ReactivateUserAsync(int id);
        Task<GetUserDto?> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}