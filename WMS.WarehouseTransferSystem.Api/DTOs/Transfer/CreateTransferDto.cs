

namespace WMS.WarehouseTransferSystem.Api.DTOs.Transfer
{
    public class CreateTransferDto
    {
        public int SourceWarehouseId { get; set; }
        public int DestinationWarehouseId { get; set; }
        public List<TransferItemDto> Item {get; set;} = new ();
    }
}