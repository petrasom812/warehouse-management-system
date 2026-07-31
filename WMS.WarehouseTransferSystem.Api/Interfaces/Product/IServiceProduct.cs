using WMS.WarehouseTransferSystem.Api.DTOs.Product;

namespace WMS.WarehouseTransferSystem.Api.Interfaces.Product
{
    public interface IServiceProduct
    {
        Task<GetProductDto> CreateProductAsync(CreateProductDto dto);
        Task<List<GetProductDto>> GetProductAsync();
        Task<GetProductDto?> GetProductByIdAsync(int id);
        Task<GetProductDto?> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);
    }
}