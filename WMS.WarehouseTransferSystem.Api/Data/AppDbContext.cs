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
    }
}