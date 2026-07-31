using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;

namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class ServiceUser : IServiceUser
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<UserModel> _hasher;
        public ServiceUser(AppDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<UserModel>();
        }
        #region CRUD Operations
        //Create User
        public async Task<GetUserDto> CreateUserAsync(CreateUserDto dto)
        {
            //Retrieve and validate business rule
            await EnsureUserAndEmailNotExistForCreate(dto.Username, dto.Email);
            //Validate input string
            ValidateString(dto.Username);
            ValidateString(dto.Email);
            ValidateString(dto.Password);
            //Mutate
            var createUser = new UserModel
            {
                Username = dto.Username,
                Email = dto.Email,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };
            //Hash password
            createUser.Password = _hasher.HashPassword(
                createUser,
                dto.Password
            );
            _context.Users.Add(createUser);
            //Persist
            await _context.SaveChangesAsync();
            //return
            return MapToUserDto(createUser);
        }
        //Get User
        public async Task<List<GetUserDto>> GetUserAsync()
        {
            //Projection: return and retrieve
            return await _context.Users
                .AsNoTracking()
                .Select(u => new GetUserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedDate = u.CreatedDate
                })
                .ToListAsync();
        }
        //Get User by ID
        public async Task<GetUserDto?> GetUserByIdAsync(int id)
        {
            //Retrieve
            var getUserById = await EnsureUserExists(id);
            //Mutate
            return MapToUserDto(getUserById);
        }
        //Update User
        public async Task<GetUserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            //validate input string
            ValidateString(dto.Username);
            ValidateString(dto.Email);
            //Validate: Username or Email does not exist
            await EnsureUserAndEmailNotExistForUpdate(id, dto.Username, dto.Email);
            //Retrieve and validate: User exsits
            var user = await EnsureUserExists(id);
            //Mutate
            user.Username = dto.Username;
            user.Email = dto.Email;
            //Persist
            await _context.SaveChangesAsync();
            //return
            return MapToUserDto(user);
        }
        //Soft delete user
        public async Task<bool> DeactivateUserAsync(int id)
        {
            //Retrieve and validate: user exists
            var user = await EnsureUserExists(id);
            //Mutate
            user.IsActive = false;
            //persist
            await _context.SaveChangesAsync();
            //return
            return true;
        }
        //Re-Activate user
        public async Task<bool> ReactivateUserAsync(int id)
        {
            //Retrieve and validate: user exists
            var user = await EnsureUserExists(id);
            //Mutate
            user.IsActive = true;
            //persist
            await _context.SaveChangesAsync();
            //return
            return true;
        }
        //Change Password
        public async Task<GetUserDto?> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            //retrieve and validate: user exists
            var user = await EnsureUserExists(userId);
            //validate input string
            ValidateString(dto.CurrentPassword);
            ValidateString(dto.NewPassword);
            //validate: password is verified
            EnsurePasswordVerified(user, dto.CurrentPassword, dto.NewPassword);
            //validate if null or whitespace
            ValidateString(dto.NewPassword);
            //hash password
            user.Password = _hasher.HashPassword(
                user,
                dto.NewPassword
            );
            //Persist
            await _context.SaveChangesAsync();
            return MapToUserDto(user);
        }
        //Hard delete User
        public async Task<bool> DeleteUserAsync(int id)
        {
            //retrieve and validate: user exists
            var user = await EnsureUserExists(id);
            //Mutate
            _context.Users.Remove(user);
            //persist
            await _context.SaveChangesAsync();
            //return
            return true;
        }
        #endregion CRUD Operations
        #region Validation Helper
        //Validate string method
        private static void ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Username or Email or Password cannot be empty.");
        }
        //Ensure user does not exist
        private async Task EnsureUserAndEmailNotExistForCreate(string username, string email)
        {
            //retrieve
            var user = await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
            //validate
            if (user)
                throw new ArgumentException("Username or password already exists.");
        }
        private async Task EnsureUserAndEmailNotExistForUpdate(int id, string username, string email)
        {
            //retrieve
            var user = await _context.Users
                .AnyAsync(u => (u.Username == username || u.Email == email) && u.Id != id);
            //validate
            if(user)
                throw new ArgumentException("Username and Email already exists.");
        }
        //Ensure user exists
        private async Task<UserModel> EnsureUserExists(int id)
        {
            //retrieve
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            //validate
            if (user == null)
                throw new KeyNotFoundException("User does not exist.");
            //return
            return user;
        }
        //Ensure password is verified(Change password)
        private void EnsurePasswordVerified(UserModel user, string currentPassword, string newPassowrd)
        {
            //retrieve
            var result = _hasher.VerifyHashedPassword(
                user,
                user.Password,
                currentPassword
            );
            //validate
            if(result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid username or password.");
            if(currentPassword == newPassowrd)
                throw new UnauthorizedAccessException("Current and new password cannot be the same.");
        }
        #endregion Validation Helper
        #region Mapper
        //Mapper method
        private static GetUserDto MapToUserDto(UserModel u) => new()
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            IsActive = u.IsActive,
            CreatedDate = u.CreatedDate
        };
        #endregion Mapper
    }
}