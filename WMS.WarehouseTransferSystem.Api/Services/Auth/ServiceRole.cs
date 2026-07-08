using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Role;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Models.Auth.Role;

namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class ServiceRole : IServiceRole
    {
        private readonly AppDbContext _context;
        public ServiceRole(AppDbContext context)
        {
            _context = context;
        }
        //Create Role
        public async Task<GetRoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            //Retrieve
            var roleExists = await _context.Roles
                .AnyAsync(r => r.RoleName == dto.RoleName);
            //Validate
            if (roleExists)
                throw new ArgumentException("Role already exists.");
            ValidateString(dto.RoleName);
            //Mutate
            var createRole = new RoleModel
            {
                RoleName = dto.RoleName
            };
            _context.Roles.Add(createRole);
            //persist
            await _context.SaveChangesAsync();

            return MapToRoleDto(createRole);
        }
        //Get Role
        public async Task<List<GetRoleDto>> GetRoleAsync()
        {
            //Retrieve
            var role = await _context.Roles
                .AsNoTracking()
                .ToListAsync();
            //Mutate
            return role.Select(MapToRoleDto).ToList();
        }
        //Get Roles by ID
        public async Task<GetRoleDto?> GetRoleByIdAsync(int id)
        {
            //Retrieve
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            //Mutate
            return role == null ? null : MapToRoleDto(role);
        }
        public async Task<UpdateRoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto)
        {
            //Validate
            await CheckDuplicateRoleAsync(dto.RoleName, id);
            //retrieve
            var role = await _context.Roles.FindAsync(id);
            //validate
            if(role == null)
                return null;
            role.RoleName = dto.RoleName;
            //persist
            await _context.SaveChangesAsync();
            return new UpdateRoleDto
            {
                RoleName = role.RoleName
            };

        }
        //Delete Role
        public async Task<bool> DeleteRoleAsync(int id)
        {
            //Retrieve
            var role = await _context.Roles.FindAsync(id);
            //Validate
            if(role == null)
                return false;
            //Mutate
            _context.Roles.Remove(role);
            //Persist
            await _context.SaveChangesAsync();
            return true;
        }
        //Validate String
        public string ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
            return value;
        }
        //Mapper
        private static GetRoleDto MapToRoleDto(RoleModel r) => new()
        {
            Id = r.Id,
            RoleName = r.RoleName
        };
        //Check duplicate
        private async Task CheckDuplicateRoleAsync(string roleName, int? roleId = null)
        {
            //Retrieve
            var roleExists = await _context.Roles
                .AnyAsync(r =>
                    r.RoleName == roleName &&
                    (!roleId.HasValue || r.Id != roleId.Value));
            //Validate      
            if(roleExists)
                throw new ArgumentException("Role already exists.");
        }
    }
}