using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Inventory;
using WMS.WarehouseTransferSystem.Api.Interfaces;
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
        //Create Inventory
        public async Task<GetInventoryDto> CreateInventoryAsync(CreateInventoryDto dto)
        {
            //Retrieve
            var warehouseExists = await _context.Warehouse
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == dto.WarehouseId);

            var productExists = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

            //validate
            if(warehouseExists == null)
                throw new ArgumentException("Warehouse must exists at least one or more.");
            if(productExists == null)
                throw new ArgumentException("Product must exists at least one or more.");
            //Retrieve
            var createInventory = new InventoryModel
            {
                WarehouseId = warehouseExists.Id,
                ProductId = productExists.Id,
                Quantity = dto.Quantity
            };
            //Mutate
            _context.Inventories.Add(createInventory);
            //Persist
            await _context.SaveChangesAsync();
            return new GetInventoryDto
            {
                Id = createInventory.Id,
                WarehouseCode = warehouseExists.WarehouseCode,
                ProductSku = productExists.Sku,
                ProductName = productExists.ProductName,
                Quantity = createInventory.Quantity
            };
        }
        //Get inventory
        public async Task<List<GetInventoryDto>> GetInventoryAsync()
        {
            //Retrieve
            var getInventories = await _context.Inventories
                .Select(i => new GetInventoryDto
                {
                    Id = i.Id,
                    WarehouseCode = i.Warehouse!.WarehouseCode,
                    ProductSku = i.Product!.Sku,
                    ProductName = i.Product.ProductName,
                    Quantity = i.Quantity        
                })
                .ToListAsync();
            //Mutate
            return getInventories;
        }
        //Get Inventory by Id
        public async Task<GetInventoryDto?> GetProductByIdAsync(int id)
        {
            //Retrieve
            var getInventoryById = await _context.Inventories
                .Where(i => i.Id == id)
                .Select(i => new GetInventoryDto
                {
                    Id = i.Id,
                    WarehouseCode = i.Warehouse!.WarehouseCode,
                    ProductSku = i.Product!.Sku,
                    ProductName = i.Product!.ProductName,
                    Quantity = i.Quantity
                })
                .FirstOrDefaultAsync();
            //Validate
            return getInventoryById == null ? null : getInventoryById;
        }
        //Update inventory
        public async Task<UpdateInventoryDto?> UpdateInventoryAsync(int id, UpdateInventoryDto dto)
        {
            var updateInventory = await _context.Inventories.FindAsync(id);
            if(updateInventory == null)
                return null;
            updateInventory.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            
            return new UpdateInventoryDto
            {
                Quantity = updateInventory.Quantity
            };
        }
        public async Task<bool> DeleteInventoryAsync(int id)
        {
            var deleteInventory = await _context.Inventories.FindAsync(id);
            if(deleteInventory == null)
                return false;
            _context.Inventories.Remove(deleteInventory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}