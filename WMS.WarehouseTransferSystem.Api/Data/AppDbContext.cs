using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Models.Inventory;
using WMS.WarehouseTransferSystem.Api.Models.Product;
using WMS.WarehouseTransferSystem.Api.Models.Transfer;
using WMS.WarehouseTransferSystem.Api.Models.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<WarehouseModel> Warehouse {get; set;}
        public DbSet<ProductModel> Products {get; set;}
        public DbSet<InventoryModel> Inventories { get; set; }
        public DbSet<TransferModel> Transfer { get; set; }
        public DbSet<TransferItemModel> TransferItems { get; set; }
    }
}