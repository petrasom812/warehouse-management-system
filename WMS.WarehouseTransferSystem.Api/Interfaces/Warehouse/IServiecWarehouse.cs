using WMS.WarehouseTransferSystem.Api.DTOs.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Interfaces
{
    public interface IServiecWarehouse
    {
        Task<GetWarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto);
        Task<List<GetWarehouseDto>> GetWarehouseDtoAsync();
        Task<GetWarehouseDto?> GetWarehouseByIdAsync(int id);
        Task<UpdateWarehouseDto?> UpdateWarehouseAsync(int id, UpdateWarehouseDto dto);
        Task<bool> DeleteWarehouseAsync(int id);
    }
}