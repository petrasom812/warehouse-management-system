using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Transfer;
using WMS.WarehouseTransferSystem.Api.Interfaces.Transfer;
using WMS.WarehouseTransferSystem.Api.Models.Inventory;
using WMS.WarehouseTransferSystem.Api.Models.Transfer;

namespace WMS.WarehouseTransferSystem.Api.Services.Transfer
{
    public class ServiceTransfer : IServiceTransfer
    {
        private readonly AppDbContext _context;
        public ServiceTransfer(AppDbContext context)
        {
            _context = context;
        }
        //Create Transfer
        public async Task<GetTransferDto> CreateTransferAsync(CreateTransferDto dto)
        {
            //Retrieve
            var sourceWarehouseExists = await _context.Warehouse
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == dto.SourceWarehouseId);
            var destinationWarehouseExists = await _context.Warehouse
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == dto.DestinationWarehouseId);
            var productExists = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => dto.Item.Select(i => i.ProductId)
                .Contains(p.Id));
            var sourceInventory = await _context.Inventories
                .Where(i =>
                    i.WarehouseId == dto.SourceWarehouseId &&
                    dto.Item.Select(it => it.ProductId)
                        .Contains(i.ProductId))
                .ToListAsync();


            //Validation
            if (sourceWarehouseExists == null)
                throw new ArgumentException("Source warehouse must exists at least one or more.");
            if (destinationWarehouseExists == null)
                throw new ArgumentException("Destination warehouse must exists at least one or more.");
            if (dto.SourceWarehouseId == dto.DestinationWarehouseId)
                throw new ArgumentException("Inventory cannot be transferred to the same type of warehouse.");
            if (productExists == null)
                throw new ArgumentException("Product must exists at least one or more.");
            if (sourceInventory == null)
                throw new ArgumentException("Source Inventory must exists at least one or more.");
            foreach (var item in dto.Item)
            {
                var inventory = sourceInventory
                    .FirstOrDefault(i => i.ProductId == item.ProductId);
                var destinationInventory = await _context.Inventories
                    .FirstOrDefaultAsync(i =>
                        i.WarehouseId == dto.DestinationWarehouseId &&
                        i.ProductId == item.ProductId);
                if (inventory == null)
                    throw new ArgumentException($"Inventory for Product {item.ProductId} does not exist.");
                if (inventory.Quantity < item.Quantity)
                    throw new ArgumentException($"Insufficient inventory for product {item.ProductId}");
                inventory.Quantity -= item.Quantity;
                if(destinationInventory == null)
                {
                    destinationInventory = new InventoryModel
                    {
                        WarehouseId = dto.DestinationWarehouseId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };
                    _context.Inventories.Add(destinationInventory);
                }
                else
                {
                    destinationInventory.Quantity += item.Quantity;
                }
                
            }
            //Mutate Parent
            var createTransfer = new TransferModel
            {
                TransferNumber = TransferNumberGenerator.GenerateTransferNumber(),
                SourceWarehouseId = sourceWarehouseExists.Id,
                DestinationWarehouseId = destinationWarehouseExists.Id
            };
            _context.Transfer.Add(createTransfer);
            //Persist Parent
            await _context.SaveChangesAsync();
            //Mutate Child
            var createTransferItem = dto.Item.Select(i => new TransferItemModel
            {
                TransferId = createTransfer.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity
            });
            //Persist
            _context.TransferItems.AddRange(createTransferItem);
            await _context.SaveChangesAsync();

            return MapToTransferDto(createTransfer);
        }
        //Get Transfer
        public async Task<List<GetTransferDto>> GetTransferAsync()
        {
            //Retrieve
            var getTransfer = await _context.Transfer
                .AsNoTracking()
                .ToListAsync();

            return getTransfer
                .Select(MapToTransferDto)
                .ToList();
        }
        //Get Transfer by Id
        public async Task<GetTransferDto?> GetTransferByIdAsync(int id)
        {
            //Retrieve
            var getTransferById = await _context.Transfer
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
            //Mutate
            return getTransferById == null ? null : MapToTransferDto(getTransferById);
        }
        //Mapper
        private static GetTransferDto MapToTransferDto(TransferModel t) => new()
        {
            Id = t.Id,
            TransferNumber = t.TransferNumber,
            SourceWarehouseId = t.SourceWarehouseId,
            DestinationWarehouseId = t.DestinationWarehouseId
        };
    }
}