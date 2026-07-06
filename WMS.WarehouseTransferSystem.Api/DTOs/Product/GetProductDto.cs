namespace WMS.WarehouseTransferSystem.Api.DTOs.Product
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }
}