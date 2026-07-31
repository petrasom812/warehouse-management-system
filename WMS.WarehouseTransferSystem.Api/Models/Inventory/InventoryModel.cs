using WMS.WarehouseTransferSystem.Api.Models.Product;
using WMS.WarehouseTransferSystem.Api.Models.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Models.Inventory
{
    public class InventoryModel
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public WarehouseModel Warehouse {get; set;} = null!;
        public int ProductId { get; set; }
        public ProductModel Product {get; set;} = null!;
        public int Quantity { get; set; }
    }
}