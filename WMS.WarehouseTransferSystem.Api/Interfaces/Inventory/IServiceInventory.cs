using WMS.WarehouseTransferSystem.Api.DTOs.Inventory;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Inventory
{
    public interface IServiceInventory
    {
        Task<GetInventoryDto> CreateInventoryAsync(CreateInventoryDto dto);
        Task<List<GetInventoryDto>> GetInventoryAsync();
        Task<GetInventoryDto?> GetInventoryByIdAsync(int id);
        Task<GetInventoryDto> UpdateInventoryAsync(int id, UpdateInventoryDto dto);
        Task<bool> DeleteInventoryAsync(int id);
    }
}