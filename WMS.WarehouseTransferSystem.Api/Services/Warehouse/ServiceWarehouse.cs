using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Warehouse;
using WMS.WarehouseTransferSystem.Api.Interfaces.Warehouse;
using WMS.WarehouseTransferSystem.Api.Models.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Services.Warehouse
{
    public class ServiceWarehouse : IServiceWarehouse
    {
        private readonly AppDbContext _context;
        public ServiceWarehouse(AppDbContext context)
        {
            _context = context;
        }
        #region CRUD Operations
        //Create Warehouse
        public async Task<GetWarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto)
        {
            //Validate input string
            ValidateString(dto.WarehouseCode);
            ValidateString(dto.WarehouseName);
            //Retrieve and validate warehouse existence
            await EnsureWarehouseNotExistForCreate(dto.WarehouseCode);
            //Mutate
            var createWarehouse = new WarehouseModel
            {
                WarehouseCode = dto.WarehouseCode,
                WarehouseName = dto.WarehouseName
            };
            _context.Warehouse.Add(createWarehouse);
            //Persist
            await _context.SaveChangesAsync();
            //return
            return MapToWarehouseDto(createWarehouse);
        }
        //Get Warehouse
        public async Task<List<GetWarehouseDto>> GetWarehousesAsync()
        {
            //return - Refactored
            return await _context.Warehouse
                .AsNoTracking()
                .Select(w => new GetWarehouseDto
                {
                    Id = w.Id,
                    WarehouseCode = w.WarehouseCode,
                    WarehouseName = w.WarehouseName
                })
                .ToListAsync();
        }
        //Get by Id
        public async Task<GetWarehouseDto?> GetWarehouseByIdAsync(int id)
        {
            //Retrieve and validate warehouse existence
            var warehouse = await EnsureWarehouseExists(id);
            //Validate and Mutate
            return MapToWarehouseDto(warehouse);
        }
        //Update Warehouse
        public async Task<GetWarehouseDto?> UpdateWarehouseAsync(int id, UpdateWarehouseDto dto)
        {
            //Validate input string
            ValidateString(dto.WarehouseCode);
            ValidateString(dto.WarehouseName);
            //Retrieve and validate warehouse does not exist
            await EnsureWarehouseNotExistForUpdate(id, dto.WarehouseCode);
            //retrieve and validate warehouse exists
            var warehouse = await EnsureWarehouseExists(id);
            //Mutate
            warehouse.WarehouseCode = dto.WarehouseCode;
            warehouse.WarehouseName = dto.WarehouseName;
            //Persist
            await _context.SaveChangesAsync();
            //return
            return MapToWarehouseDto(warehouse);
        }
        //Delete Warehouse
        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            //Retrieve and validate warehouse exists
            var warehouse = await EnsureWarehouseExists(id);
            //Mutate
            _context.Warehouse.Remove(warehouse);
            //Persist
            await _context.SaveChangesAsync();
            //return
            return true;
        }
        #endregion CRUD Operations
        #region Validation Helper
        //Ensure string input correctly
        private static void ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
        }
        //Ensure Warehouse does not exist for Create
        private async Task EnsureWarehouseNotExistForCreate(string warehouseCode)
        {
            //retrieve
            var warehouse = await _context.Warehouse
                .AnyAsync(w => w.WarehouseCode == warehouseCode);
            //validate
            if (warehouse)
                throw new ArgumentException("Warehouse code or name already exists.");
        }
        //Ensure Warehouse does not exist for Update
        private async Task EnsureWarehouseNotExistForUpdate(int id, string warehouseCode)
        {
            //retreive
            var warehouse = await _context.Warehouse
                .AnyAsync(w => w.Id != id &&
                    w.WarehouseCode == warehouseCode);
            //validate
            if (warehouse)
                throw new ArgumentException("Warehouse code or name already exists.");
        }
        //Ensure Warehouse exists
        private async Task<WarehouseModel> EnsureWarehouseExists(int id)
        {
            //retrieve
            var warehouse = await _context.Warehouse
                .FirstOrDefaultAsync(w => w.Id == id);
            //validate
            if (warehouse == null)
                throw new KeyNotFoundException("Warehouse does not exist.");
            //return
            return warehouse;
        }
        #endregion Validate Helper
        #region Mapper
        //Mapper
        private static GetWarehouseDto MapToWarehouseDto(WarehouseModel w) => new()
        {
            Id = w.Id,
            WarehouseCode = w.WarehouseCode,
            WarehouseName = w.WarehouseName
        };
        #endregion Mapper
    }
}