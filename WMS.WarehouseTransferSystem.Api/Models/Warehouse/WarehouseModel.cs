    using WMS.WarehouseTransferSystem.Api.Models.Inventory;
using WMS.WarehouseTransferSystem.Api.Models.Transfer;

namespace WMS.WarehouseTransferSystem.Api.Models.Warehouse
    {
        public class WarehouseModel
        {
            public int Id { get; set; }
            public string WarehouseCode { get; set; } = string.Empty;
            public string WarehouseName { get; set; } = string.Empty;
            public ICollection<InventoryModel> Inventories { get; set; } = new List<InventoryModel>();
            public ICollection<TransferModel> SourceTransfers { get; set; } = new List<TransferModel>();
            public ICollection<TransferModel> DestinationTransfers { get; set; } = new List<TransferModel>();
        }
    }