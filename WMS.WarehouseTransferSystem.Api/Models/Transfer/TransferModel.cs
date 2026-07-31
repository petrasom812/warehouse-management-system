using WMS.WarehouseTransferSystem.Api.Models.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Models.Transfer
{
    public class TransferModel
    {
        public int Id { get; set; }
        public string TransferNumber { get; set; } = string.Empty;
        public int SourceWarehouseId { get; set; }
        public int DestinationWarehouseId { get; set; }
        public WarehouseModel SourceWarehouse { get; set; } = null!;
        public WarehouseModel DestinationWarehouse { get; set; } = null!;
        public ICollection<TransferItemModel> TransferItems { get; set; } = new List<TransferItemModel>();
    }
}