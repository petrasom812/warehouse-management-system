using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Auth
{
    public interface IServiceJwt
    {
        Task<TokenResponseDto> JwtTokenAsync(UserModel user);
    }
}