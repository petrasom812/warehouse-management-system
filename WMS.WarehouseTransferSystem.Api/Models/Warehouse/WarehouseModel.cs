namespace WMS.WarehouseTransferSystem.Api.Models.Warehouse
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public string WarehouseCode { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;

    }
}