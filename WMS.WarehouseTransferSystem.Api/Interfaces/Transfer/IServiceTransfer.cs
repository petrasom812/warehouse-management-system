
using WMS.WarehouseTransferSystem.Api.DTOs.Transfer;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Transfer
{
    public interface IServiceTransfer
    {
        Task<GetTransferDto> CreateTransferAsync(CreateTransferDto dto);
        Task<List<GetTransferDto>> GetTransferAsync();
        Task<GetTransferDto?> GetTransferByIdAsync(int id);
    }
}