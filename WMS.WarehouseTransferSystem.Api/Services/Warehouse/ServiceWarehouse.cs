using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Warehouse;
using WMS.WarehouseTransferSystem.Api.Interfaces;
using WMS.WarehouseTransferSystem.Api.Models.Warehouse;

namespace WMS.WarehouseTransferSystem.Api.Services.Warehouse
{
    public class ServiceWarehouse : IServiecWarehouse
    {
        private readonly AppDbContext _context;
        public ServiceWarehouse(AppDbContext context)
        {
            _context = context;
        }
        //Create Warehouse
        public async Task<GetWarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto)
        {
            //Validate string value
            ValidateString(dto.WarehouseCode);
            ValidateString(dto.WarehouseName);
            //Retrieve
            var createWarehouse = new WarehouseModel
            {
                WarehouseCode = dto.WarehouseCode,
                WarehouseName = dto.WarehouseName
            };
            //Mutate
            _context.Warehouse.Add(createWarehouse);
            //Persist
            await _context.SaveChangesAsync();

            return MapToWarehouseDto(createWarehouse);
        }
        //Get Warehouse
        public async Task<List<GetWarehouseDto>> GetWarehouseDtoAsync()
        {
            //Retrieve
            var getWarehouse = await _context.Warehouse
                .AsNoTracking()
                .ToListAsync();
            
            //Mutate
            return getWarehouse
                .Select(MapToWarehouseDto)
                .ToList();
        }
        //Get by Id
        public async Task<GetWarehouseDto?> GetWarehouseByIdAsync(int id)
        {
            //Retrieve
            var getWarehouseById = await _context.Warehouse
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
            
            //Validate and Mutate
            return getWarehouseById == null ? null : MapToWarehouseDto(getWarehouseById);
        }
        //Update Warehouse
        public async Task<UpdateWarehouseDto?> UpdateWarehouseAsync(int id, UpdateWarehouseDto dto)
        {
            //Retrieve
            var updateWarehouse = await _context.Warehouse.FindAsync(id);
            //Validate
            if(updateWarehouse == null)
                return null;
            ValidateString(dto.WarehouseCode);
            ValidateString(dto.WarehouseName);
            //Mutate
            updateWarehouse.WarehouseCode = dto.WarehouseCode;
            updateWarehouse.WarehouseName = dto.WarehouseName;
            //Persist
            await _context.SaveChangesAsync();

            return new UpdateWarehouseDto
            {
                WarehouseCode = updateWarehouse.WarehouseCode,
                WarehouseName = updateWarehouse.WarehouseName
            };
        }
        //Delete Warehouse
        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            //Retrieve
            var deleteWarehouse = await _context.Warehouse.FindAsync(id);
            //Validate
            if(deleteWarehouse == null)
                return false;
            //Mutate
            _context.Warehouse.Remove(deleteWarehouse);
            //Persist
            await _context.SaveChangesAsync();

            return true;
        }
        //Mapper
        private static GetWarehouseDto MapToWarehouseDto(WarehouseModel w) => new()
        {
            Id = w.Id,
            WarehouseCode = w.WarehouseCode,
            WarehouseName = w.WarehouseName
        };
        //String validation
        public string ValidateString(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
            return value;
        }
    }
}