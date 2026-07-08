using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Models.Auth.UserRole;

namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class ServiceUserRole : IServiceUserRole
    {
        private readonly AppDbContext _context;
        public ServiceUserRole(AppDbContext context)
        {
            _context = context;
        }
        //Create UserRole
        public async Task<GetUserRoleDto> CreateUserRolelAsync(CreateUserRoleDto dto)
        {
            //Retrieve
            var userExists = await _context.Users
                .AnyAsync(u =>
                    u.Id == dto.UserId);
            var roleExists = await _context.Roles
                .AnyAsync(r => 
                    r.Id == dto.RoleId);
            var assignmentExists = await _context.UserRoles
                .AnyAsync(ur =>
                    ur.UserId == dto.UserId &&
                    ur.RoleId == dto.RoleId);
            //Validate
            if(!userExists)
                throw new ArgumentException("User not exists.");
            if(!roleExists)
                throw new ArgumentException("Role not exists.");
            if(assignmentExists)
                throw new ArgumentException("User is already assigned to this role.");
            //Mutate
            var createUserRole = new UserRoleModel
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };
            _context.UserRoles.Add(createUserRole);
            //Persist
            await _context.SaveChangesAsync();

            var result = await _context.UserRoles
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.Id == createUserRole.Id);

            return MapToUserRoleDto(createUserRole);
        }
        //Get Role
        public async Task<List<GetUserRoleDto>> GetUserRoleAsync()
        {
            //retrieve
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();
            //Mutate
            return userRole
                .Select(MapToUserRoleDto)
                .ToList();
        }
        public async Task<GetUserRoleDto?> GetUserRoleByIdAsync(int id)
        {
            //retrieve
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id);
            //Validate & Mutate
            return userRole == null ? null : MapToUserRoleDto(userRole);
        }
        //Mapper
        private static GetUserRoleDto MapToUserRoleDto(UserRoleModel u) => new()
        {
            UserId = u.UserId,
            RoleId = u.RoleId,
            RoleName = u.Role?.RoleName ?? ""
        };
    }
}