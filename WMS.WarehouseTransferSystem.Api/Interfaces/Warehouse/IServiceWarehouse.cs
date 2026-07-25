using WMS.WarehouseTransferSystem.Api.DTOs.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Warehouse
{
    public interface IServiceWarehouse
    {
        Task<GetWarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto);
        Task<List<GetWarehouseDto>> GetWarehousesAsync();
        Task<GetWarehouseDto?> GetWarehouseByIdAsync(int id);
        Task<GetWarehouseDto?> UpdateWarehouseAsync(int id, UpdateWarehouseDto dto);
        Task<bool> DeleteWarehouseAsync(int id);
    }
}