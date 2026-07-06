namespace WMS.WarehouseTransferSystem.Api.DTOs.Inventory
{
    public class GetInventoryDto
    {
        public int Id { get; set; }
        public string WarehouseCode { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}