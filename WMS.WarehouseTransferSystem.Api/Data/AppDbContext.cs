using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Models.Auth.Role;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;
using WMS.WarehouseTransferSystem.Api.Models.Auth.UserRole;
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
        public DbSet<UserModel> Users {get; set;}
        public DbSet<RoleModel> Roles {get; set;}
        public DbSet<UserRoleModel> UserRoles {get; set;}

        //relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Inventory -> Warehouse
            modelBuilder.Entity<InventoryModel>()
            .HasOne(i => i.Warehouse)
            .WithMany(w => w.Inventories)
            .HasForeignKey(i => i.WarehouseId);

            //Inventory -> Product
            modelBuilder.Entity<InventoryModel>()
            .HasOne(i => i.Product)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductId);

            //Transfer -> Source Warehouse
            modelBuilder.Entity<TransferModel>()
            .HasOne(t => t.SourceWarehouse)
            .WithMany(w => w.SourceTransfers)
            .HasForeignKey(t => t.SourceWarehouseId);

            //Transfer -> Destination Warhouse
            modelBuilder.Entity<TransferModel>()
            .HasOne(t => t.DestinationWarehouse)
            .WithMany(w => w.DestinationTransfers)
            .HasForeignKey(t => t.DestinationWarehouseId);

            //TransferItem -> Transfer
            modelBuilder.Entity<TransferItemModel>()
            .HasOne(ti => ti.Transfer)
            .WithMany(t => t.TransferItems)
            .HasForeignKey(ti => ti.TransferId);

            //TransferItem -> Product
            modelBuilder.Entity<TransferItemModel>()
            .HasOne(ti => ti.Product)
            .WithMany(p => p.TransferItems)
            .HasForeignKey(ti => ti.ProductId);
        }
    }
}