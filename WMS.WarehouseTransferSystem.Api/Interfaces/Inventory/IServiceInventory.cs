
using WMS.WarehouseTransferSystem.Api.DTOs.Inventory;

namespace WMS.WarehouseTransferSystem.Api.Interfaces
{
    public interface IServiceInventory
    {
        Task<GetInventoryDto> CreateInventoryAsync(CreateInventoryDto dto);
        Task<List<GetInventoryDto>> GetInventoryAsync();
        Task<GetInventoryDto?> GetProductByIdAsync(int id);
        Task<UpdateInventoryDto?> UpdateInventoryAsync(int id, UpdateInventoryDto dto);
        Task<bool> DeleteInventoryAsync(int id);
    }
}