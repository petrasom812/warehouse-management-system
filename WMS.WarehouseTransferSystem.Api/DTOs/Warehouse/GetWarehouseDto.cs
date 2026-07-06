

namespace WMS.WarehouseTransferSystem.Api.DTOs.Warehouse
{
    public class GetWarehouseDto
    {
        public int Id { get; set; }
        public string WarehouseCode { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
    }
}