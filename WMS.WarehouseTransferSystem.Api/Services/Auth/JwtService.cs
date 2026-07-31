using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth.Token;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Settings;
using Microsoft.Extensions.Options;


namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class JwtService : IServiceJwt
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSetting;
        public JwtService(
            AppDbContext context,
            IOptions<JwtSettings> options)
        {
            _context = context;
            _jwtSetting = options.Value;
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
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiryMinute);
            //Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _jwtSetting.Key!
                )
            );
            //Credential
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            //Create Token
            var token = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
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