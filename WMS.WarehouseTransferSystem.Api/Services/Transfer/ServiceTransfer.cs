using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Transfer;
using WMS.WarehouseTransferSystem.Api.Interfaces.Transfer;
using WMS.WarehouseTransferSystem.Api.Models.Inventory;
using WMS.WarehouseTransferSystem.Api.Models.Transfer;
using WMS.WarehouseTransferSystem.Api.Models.Warehouse;


namespace WMS.WarehouseTransferSystem.Api.Services.Transfer
{
    public class ServiceTransfer : IServiceTransfer
    {
        private readonly AppDbContext _context;
        public ServiceTransfer(AppDbContext context)
        {
            _context = context;
        }
        #region CRUD Operations
        //Create Transfer
        public async Task<GetTransferDto> CreateTransferAsync(CreateTransferDto dto)
        {
            //Retrieve
            var inventories = await RetrieveTransferInventories(dto.SourceWarehouseId, dto.DestinationWarehouseId, dto.Item.Select(p => p.ProductId));
            await EnsureSourceAndDestinationExists(dto.SourceWarehouseId, dto.DestinationWarehouseId);
            await EnsureProductsExist(dto.Item.Select(p => p.ProductId));
            //validation
            foreach (var item in dto.Item)
            {
                var sourceInventory = inventories
                    .FirstOrDefault(i =>
                        i.WarehouseId == dto.SourceWarehouseId &&
                        i.ProductId == item.ProductId);
                var destinationInventory = inventories
                    .FirstOrDefault(i =>
                        i.WarehouseId == dto.DestinationWarehouseId &&
                        i.ProductId == item.ProductId);
                if (sourceInventory == null)
                    throw new ArgumentException($"Inventory for Product {item.ProductId} does not exist.");
                if (sourceInventory.Quantity < item.Quantity)
                    throw new ArgumentException($"Insufficient inventory for product {item.ProductId}");
                sourceInventory.Quantity -= item.Quantity;
                if (destinationInventory == null)
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
                SourceWarehouseId = dto.SourceWarehouseId,
                DestinationWarehouseId = dto.DestinationWarehouseId
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
            //Retrieve and return: Projection
            return await _context.Transfer
                .AsNoTracking()
                .Select(t => new GetTransferDto
                {
                    Id = t.Id,
                    TransferNumber = t.TransferNumber,
                    SourceWarehouseId = t.SourceWarehouseId,
                    DestinationWarehouseId = t.DestinationWarehouseId
                })
                .ToListAsync();
        }
        //Get Transfer by Id
        public async Task<GetTransferDto?> GetTransferByIdAsync(int id)
        {
            //Retrieve
            var transfer = await EnsureTransferExists(id);
            //Mutate
            return MapToTransferDto(transfer);
        }
        #endregion CRUD Operations
        #region Validation Helper
        //Ensure warehouse exists both Source and Destination
        private async Task<List<WarehouseModel>> EnsureSourceAndDestinationExists(int sourceId, int destinationId)
        {
            //retrieve
            var warehouse = await _context.Warehouse
                .Where(w => w.Id == sourceId || w.Id == destinationId)
                .ToListAsync();
            //validate
            if (warehouse.Count != 2)
                throw new KeyNotFoundException("Source and Destination warehouse does not exist.");
            if (sourceId == destinationId)
                throw new ArgumentException("Source and Destination warehouse cannot be the same.");
            //return
            return warehouse;
        }
        //Ensure Product exists
        private async Task EnsureProductsExist(IEnumerable<int> productId)
        {
            //retrieve
            var product = await _context.Products
                .CountAsync(p => productId.Contains(p.Id));
            //validate
            if(product != productId.Distinct().Count())
                throw new KeyNotFoundException("Product does not exist.");
        }
        //Retrieve Inventory
        private async Task<List<InventoryModel>> RetrieveTransferInventories(int sourceId, int destinationId, IEnumerable<int> productId)
        {
            return await _context.Inventories
                .Where(i =>
                    (i.WarehouseId == sourceId ||
                    i.WarehouseId == destinationId)
                    &&
                    productId.Contains(i.ProductId)
                ).ToListAsync();
        }
        //Ensure Transfer Id exists
        private async Task<TransferModel> EnsureTransferExists(int id)
        {
            //retrieve
            var transfer = await _context.Transfer
                .FirstOrDefaultAsync(t => t.Id == id);
            //validate
            if(transfer == null)
                throw new KeyNotFoundException("Transfer Id does not exist.");
            //return
            return transfer;
        }
        #endregion Validation Helper
        #region Mapper
        //Mapper
        private static GetTransferDto MapToTransferDto(TransferModel t) => new()
        {
            Id = t.Id,
            TransferNumber = t.TransferNumber,
            SourceWarehouseId = t.SourceWarehouseId,
            DestinationWarehouseId = t.DestinationWarehouseId
        };
        #endregion Mapper
    }
}