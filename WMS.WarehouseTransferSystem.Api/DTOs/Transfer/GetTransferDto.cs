namespace WMS.WarehouseTransferSystem.Api.DTOs.Transfer
{
    public class GetTransferDto
    {
        public int Id { get; set; }
        public string TransferNumber { get; set; } = string.Empty;
        public int SourceWarehouseId { get; set; }
        public int DestinationWarehouseId { get; set; }
    }
}