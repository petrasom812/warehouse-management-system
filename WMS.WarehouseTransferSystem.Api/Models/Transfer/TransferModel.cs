namespace WMS.WarehouseTransferSystem.Api.Models.Transfer
{
    public class TransferModel
    {
        public int Id { get; set; }
        public string TransferNumber { get; set; } = string.Empty;
        public int SourceWarehouseId { get; set; }
        public int DestinationWarehouseId { get; set; }
    }
}