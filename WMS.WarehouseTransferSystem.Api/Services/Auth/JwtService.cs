using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;


namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class JwtService : IServiceJwt
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public JwtService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration =configuration;
        }
        public async Task<TokenResponseDto> JwtTokenAsync(UserModel user)
        {
            //retrieve user
            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();
            //Create Claim
            var claimName = new Claim(ClaimTypes.Name, user.Username);
            var claimId = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            var claimRole = userRole
                .Select(role => 
                    new Claim(
                        ClaimTypes.Role,
                        role
                    ))
                .ToList();
            var claims = new List<Claim>
            {
                claimName,
                claimId
            };
            claims.AddRange(claimRole);
            //expiration
            var expiration = DateTime.UtcNow.AddHours(8);
            //Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );
            //Credential
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            //Create Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );
            //Token string
            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);
            
            return new TokenResponseDto
            {
                Token = tokenString,
                Expiration = expiration
            };
        }
    }
}