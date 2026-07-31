using WMS.WarehouseTransferSystem.Api.Models.Inventory;
using WMS.WarehouseTransferSystem.Api.Models.Transfer;

namespace WMS.WarehouseTransferSystem.Api.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public ICollection<InventoryModel> Inventories { get; set; } = new List<InventoryModel>();
        public ICollection<TransferItemModel> TransferItems { get; set; } = new List<TransferItemModel>();
    }
}