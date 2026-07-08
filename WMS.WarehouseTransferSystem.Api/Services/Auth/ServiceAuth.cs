using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Login;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.LoginDto;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;

namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class ServiceAuth : IServiceAuth
    {
        private readonly AppDbContext _context;
        private readonly IServiceJwt _serviceJwt;
        public ServiceAuth(
            AppDbContext context,
            IServiceJwt serviceJwt)
        {
            _context = context;
            _serviceJwt = serviceJwt;
        }
        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            //validate string
            ValidateString(dto.Username);
            ValidateString(dto.Password);
            //Retrieve
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);
            //validate
            if (user == null)
                throw new ArgumentException("Invalid username or password.");
            if (!user.IsActive)
                throw new ArgumentException("User is inactive");

            //retrieve
            var hasher = new PasswordHasher<UserModel>();
            var result = hasher.VerifyHashedPassword(
                user,
                user.Password,
                dto.Password
            );
            //validate
            if (result == PasswordVerificationResult.Failed)
                throw new ArgumentException("Invalid username or password.");

            return await _serviceJwt.JwtTokenAsync(user);
    
        }
        //Validate String
        public string ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
            return value;
        }
    }
}