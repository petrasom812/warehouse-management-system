
namespace WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token
{
    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}