using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Inventory;
using WMS.WarehouseTransferSystem.Api.Interfaces.Inventory;
using WMS.WarehouseTransferSystem.Api.Models.Inventory;

namespace WMS.WarehouseTransferSystem.Api.Services.Inventory
{
    public class ServiceInventory : IServiceInventory
    {
        private readonly AppDbContext _context;
        public ServiceInventory(AppDbContext context)
        {
            _context = context;
        }
        #region CRUD Operation
        //Create Inventory
        public async Task<GetInventoryDto> CreateInventoryAsync(CreateInventoryDto dto)
        {
            //Retrieve and validate
            await EnsureWarehouseAndProductExistsForCreate(dto.WarehouseId, dto.ProductId);
            await EnsureInventoryNotExist(dto.WarehouseId, dto.ProductId);
            //Mutate
            var createInventory = new InventoryModel
            {
                WarehouseId = dto.WarehouseId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            _context.Inventories.Add(createInventory);
            //Persist
            await _context.SaveChangesAsync();
            //return
            var returnDetail = await RetrieveInventoryDetail(createInventory.Id);
            return MapToInventoryDto(returnDetail);
        }
        //Get inventory
        public async Task<List<GetInventoryDto>> GetInventoryAsync()
        {
            //retrieve and return
            return await _context.Inventories
                .Include(i => i.Warehouse)
                .Include(i => i.Product)
                .Select(i => new GetInventoryDto
                {
                    Id = i.Id,
                    WarehouseCode = i.Warehouse.WarehouseCode,
                    ProductSku = i.Product.Sku,
                    ProductName = i.Product.ProductName,
                    Quantity = i.Quantity
                })
                .ToListAsync();
        }
        //Get Inventory by Id
        public async Task<GetInventoryDto?> GetInventoryByIdAsync(int id)
        {
            //retrieve and validate
            var inventory = await EnsureInventoryExists(id);
            //return
            return MapToInventoryDto(inventory);
        }
        //Update inventory
        public async Task<GetInventoryDto> UpdateInventoryAsync(int id, UpdateInventoryDto dto)
        {
            //retrieve and validate
            var inventory = await EnsureInventoryExists(id);
            //mutate
            inventory.Quantity = dto.Quantity;
            //persist
            await _context.SaveChangesAsync();
            //return
            return MapToInventoryDto(inventory);
        }
        public async Task<bool> DeleteInventoryAsync(int id)
        {
            //retrieve
            var inventory = await EnsureInventoryExists(id);
            //mutate
            _context.Inventories.Remove(inventory);
            //persist
            await _context.SaveChangesAsync();
            //return
            return true;
        }
        #endregion CRUD Operation
        #region Validation Helper
        //Ensure Warehouse exists and product exists
        private async Task EnsureWarehouseAndProductExistsForCreate(int warehouseId, int productId)
        {
            //retrieve
            var warehouse = await _context.Warehouse
                .AnyAsync(w => w.Id == warehouseId);
            var product = await _context.Products
                .AnyAsync(p => p.Id == productId);
            //validate
            if (!warehouse)
                throw new KeyNotFoundException("Warehouse does not exists.");
            if (!product)
                throw new KeyNotFoundException("Product does not exist.");
        }
        //Ensure No inventories duplication
        private async Task EnsureInventoryNotExist(int warehouseId, int productId)
        {
            //retrieve
            var inventory = await _context.Inventories
                .AnyAsync(i => i.WarehouseId == warehouseId && i.ProductId == productId);
            //validate
            if(inventory)
                throw new ArgumentException("Inventory already exists.");
        }
        //Ensure Inventory exists
        private async Task<InventoryModel> EnsureInventoryExists(int id)
        {
            //retrieve
            var inventory = await _context.Inventories
                .Include(i => i.Warehouse)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
            //validate
            if(inventory == null)
                throw new KeyNotFoundException("Inventory does not exist.");
            //return
            return inventory;
        }
        #endregion Validation Helper
        #region Mapper
        private static GetInventoryDto MapToInventoryDto(InventoryModel i) => new()
        {
            Id = i.Id,
            WarehouseCode = i.Warehouse.WarehouseCode,
            ProductSku = i.Product.Sku,
            ProductName = i.Product.ProductName,
            Quantity = i.Quantity
        };
        #endregion Mapper
        #region Retrieve Inventory Detail
        private async Task<InventoryModel> RetrieveInventoryDetail(int invId)
        {
            //retreive
            var inventory = await _context.Inventories
                .Include(i => i.Warehouse)
                .Include(i => i.Product)
                .FirstAsync(i => i.Id == invId);
            //return
            return inventory;
        }
        #endregion Retrieve Invetory Detail
    }
}