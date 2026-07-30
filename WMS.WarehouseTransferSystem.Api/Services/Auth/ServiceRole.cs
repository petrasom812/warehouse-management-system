using System.Security.Cryptography.X509Certificates;
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
        #region CRUD Operations
        //Create Role
        public async Task<GetRoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            //Retrieve and validate
            await EnsureRoleNameNotExistForCreate(dto.RoleName);
            ValidateString(dto.RoleName);
            //Mutate
            var createRole = new RoleModel
            {
                RoleName = dto.RoleName
            };
            _context.Roles.Add(createRole);
            //persist
            await _context.SaveChangesAsync();
            //return
            return MapToRoleDto(createRole);
        }
        //Get Role
        public async Task<List<GetRoleDto>> GetRoleAsync()
        {
            //Projection: Retrieve and Return
            return await _context.Roles
                .AsNoTracking()
                .Select(r => new GetRoleDto
                {
                    Id = r.Id,
                    RoleName = r.RoleName
                })
                .ToListAsync();
        }
        //Get Roles by ID
        public async Task<GetRoleDto?> GetRoleByIdAsync(int id)
        {
            //Retrieve and validate
            var role = await EnsureRoleExists(id);
            //Mutate
            return MapToRoleDto(role);
        }
        public async Task<GetRoleDto?> UpdateRoleAsync(int id, UpdateRoleDto dto)
        {
            //Validdate: Role name does not exist
            await EnsureRoleNameNotExistForUpdate(id, dto.RoleName);
            //retrieve and validate: role exists
            var role = await EnsureRoleExists(id);
            //Mutate
            role.RoleName = dto.RoleName;
            //persist
            await _context.SaveChangesAsync();
            //return
            return MapToRoleDto(role);
        }
        //Delete Role
        public async Task<bool> DeleteRoleAsync(int id)
        {
            //Retrieve and validate: Ensure role exists
            var role = await EnsureRoleExists(id);
            //Mutate
            _context.Roles.Remove(role);
            //Persist
            await _context.SaveChangesAsync();
            //return
            return true;
        }
        #endregion CRUD Operations
        #region Validation Helper
        //Ensure sting is input correctly
        public string ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Rolename cannot be null or empty.");
            return value;
        }
        //Ensure Rolename does not exists for Create
        private async Task EnsureRoleNameNotExistForCreate(string roleName)
        {
            //retrieve
            var role = await _context.Roles
                .AnyAsync(r => r.RoleName == roleName);
            //validate
            if(role)
                throw new ArgumentException("Role already exists.");
        }
        //Ensure Rolename does not exists for Update
        private async Task EnsureRoleNameNotExistForUpdate(int id, string roleName)
        {
            //retrieve
            var role = await _context.Roles
                .AnyAsync(r => r.RoleName == roleName && r.Id != id);
            //validate
            if(role)
                throw new ArgumentException("Role already exists.");
        }
        //Ensure Role exists (filter by Id)
        private async Task<RoleModel> EnsureRoleExists(int id)
        {
            //retrieve
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id);
            //validate
            if(role == null)
                throw new KeyNotFoundException("Role does not exist.");
            //return
            return role;
        }
        #endregion Validation Helper
        #region Mapper
        //Mapper
        private static GetRoleDto MapToRoleDto(RoleModel r) => new()
        {
            Id = r.Id,
            RoleName = r.RoleName
        };
        #endregion Mapper
    }
}