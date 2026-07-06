
namespace WMS.WarehouseTransferSystem.Api.Models.Transfer
{
    public class TransferItemModel
    {
        public int Id { get; set; }
        public int TransferId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}