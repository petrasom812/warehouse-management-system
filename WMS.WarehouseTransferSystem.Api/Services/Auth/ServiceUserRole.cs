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
        #region CRUD Operations
        //Create UserRole
        public async Task<GetUserRoleDto> CreateUserRolelAsync(CreateUserRoleDto dto)
        {
            //retrieve and validate: user and role exists
            await EnsureUserAndRoleExistsAndActive(dto.UserId, dto.RoleId);
            await EnsureUserRoleNotExist(dto.UserId, dto.RoleId);
            //Mutate
            var createUserRole = new UserRoleModel
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };
            _context.UserRoles.Add(createUserRole);
            //Persist
            await _context.SaveChangesAsync();

            var resultDetail = await RetrieveUserRoleDetail(createUserRole.Id);

            return MapToUserRoleDto(resultDetail);
        }
        //Get Role
        public async Task<List<GetUserRoleDto>> GetUserRoleAsync()
        {
            //Projection: return and retrieve
            return await _context.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .AsNoTracking()
                .Select(ur => new GetUserRoleDto
                {
                    Username = ur.User.Username,
                    Email = ur.User.Email,
                    RoleName = ur.Role.RoleName
                })
                .ToListAsync();
        }
        public async Task<GetUserRoleDto?> GetUserRoleByIdAsync(int id)
        {
            //retrieve and validate
            var userRole = await EnsureUserRoleExists(id);
            //return
            return MapToUserRoleDto(userRole);
        }
        #endregion CRUD Operations
        #region Validation Helper
        //Ensure User and Role exists
        private async Task EnsureUserAndRoleExistsAndActive(int userId, int roleId)
        {
            //retrieve
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new {u.IsActive})
                .FirstOrDefaultAsync();
            var role = await _context.Roles
                .AnyAsync(r => r.Id == roleId);
            //validate
            if(user == null)
                throw new KeyNotFoundException("User does not exist.");
            if(!user.IsActive)
                throw new ArgumentException("User is inactive.");
            if(!role)
                throw new KeyNotFoundException("Role does not exist.");
        }
        //Ensure User role does not exist
        private async Task EnsureUserRoleNotExist(int userId, int roleId)
        {
            //retrieve
            var userRole = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId &&
                ur.RoleId == roleId);
            //validate
            if(userRole)
                throw new ArgumentException("Use role already assigned.");
        }
        //Ensure UserRole exists
        private async Task<UserRoleModel> EnsureUserRoleExists(int id)
        {
            //retrieve
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .FirstOrDefaultAsync(ur => ur.Id == id);
            //validate
            if(userRole == null)
                throw new KeyNotFoundException("User role does not exist.");
            //return
            return userRole;
        }
        //Retrieve User Role details
        private async Task<UserRoleModel> RetrieveUserRoleDetail(int id)
        {
            //retrieve
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .FirstAsync(ur => ur.Id == id);
            //return
            return userRole;
        }
        #endregion Validation Helper
        #region Mapper
        //Mapper
        private static GetUserRoleDto MapToUserRoleDto(UserRoleModel u) => new()
        {
            Username = u.User.Username,
            Email = u.User.Email,
            RoleName = u.Role.RoleName
        };
        #endregion Mapper
    }
}