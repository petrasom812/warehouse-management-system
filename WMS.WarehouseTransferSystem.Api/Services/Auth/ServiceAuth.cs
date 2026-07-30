using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.LoginDto;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;

namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class ServiceAuth : IServiceAuth
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<UserModel> _hasher;
        private readonly IServiceJwt _serviceJwt;
        public ServiceAuth(
            AppDbContext context,
            IServiceJwt serviceJwt)
        {
            _context = context;
            _hasher = new PasswordHasher<UserModel>();
            _serviceJwt = serviceJwt;
        }
        #region CRUD Operation
        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            //validate input string
            ValidateString(dto.Username);
            ValidateString(dto.Password);
            //Retrieve and validate user
            var user = await EnsureUserExistsAndUserActiveAndUserHasRole(dto.Username);
            //Retrieve and validate password
            EnsurePasswordVerified(user, dto.Password);
            //return
            return await _serviceJwt.JwtTokenAsync(user);
        }
        #endregion CRUD Operation
        #region Validation Helper
        //Ensure string input correctly
        private static void ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Credential cannot be empty.");
        }
        #endregion Validation Helper
        //Ensure User exists and active and role assinged
        private async Task<UserModel> EnsureUserExistsAndUserActiveAndUserHasRole(string username)
        {
            //retrieve
            var user = await _context.Users
                .Include(u => u.UserRole)
                    .ThenInclude(r => r.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
            //validate
            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid username or password.");
            //retrieve
            var roles = user.UserRole
                .Select(ur => ur.Role.RoleName)
                .ToList();
            //validate
            if (!roles.Any())
                throw new UnauthorizedAccessException("User has no authorized role assigned.");
            //return
            return user;
        }
        //Ensure password verified
        private void EnsurePasswordVerified(UserModel user, string password)
        {
            //retrieve
            var result = _hasher.VerifyHashedPassword(
                user,
                user.Password,
                password    
            );
            //validate
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid username or password.");
        }
    }
}