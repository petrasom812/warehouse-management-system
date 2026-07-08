using WMS.WarehouseTransferSystem.Api.DTOs.Auth.LoginDto;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Auth
{
    public interface IServiceAuth
    {
        Task<TokenResponseDto> LoginAsync(LoginDto dto);
    }
}