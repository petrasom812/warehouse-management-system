using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WMS.WarehouseTransferSystem.Api.Data;
using WMS.WarehouseTransferSystem.Api.DTOs.Auth;
using WMS.WarehouseTransferSystem.Api.Interfaces.Auth;
using WMS.WarehouseTransferSystem.Api.Models.Auth.User;

namespace WMS.WarehouseTransferSystem.Api.Services.Auth
{
    public class ServiceUser : IServiceUser
    {
        private readonly AppDbContext _context;
        public ServiceUser(AppDbContext context)
        {
            _context = context;
        }
        //Create User
        public async Task<GetUserDto> CreateUserAsync(CreateUserDto dto)
        {
            //Retrieve
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);
            //Validate business rulle
            if (userExists)
                throw new ArgumentException("User already exists. please input a different crudential.");
            //Validate String
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

            var hasher = new PasswordHasher<UserModel>();
            createUser.Password = hasher.HashPassword(
                createUser,
                dto.Password
            );
            _context.Users.Add(createUser);
            //Persist
            await _context.SaveChangesAsync();
            return MapToUserDto(createUser);
        }
        //Get User
        public async Task<List<GetUserDto>> GetUserAsync()
        {
            //Retrieve
            var getUser = await _context.Users
                .AsNoTracking()
                .ToListAsync();
            //Mutate
            return getUser
                .Select(MapToUserDto)
                .ToList();
        }
        //Get User by ID
        public async Task<GetUserDto?> GetUserByIdAsync(int id)
        {
            //Retrieve
            var getUserById = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            //Mutate
            return getUserById == null ? null : MapToUserDto(getUserById);
        }
        //Update User
        public async Task<UpdateUserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            //Retrieve
            var usernameExists = await _context.Users
                .AnyAsync(u =>
                    u.Username == dto.Username &&
                    u.Id != id);
            var updateUser = await _context.Users.FindAsync(id);
            //Validate
            if(usernameExists)
                throw new ArgumentException("Username already exists.");
            if (updateUser == null)
                return null;
            //Mutate
            updateUser.Username = dto.Username;
            updateUser.Email = dto.Email;

            //Persist
            await _context.SaveChangesAsync();

            return new UpdateUserDto
            {
                Username = updateUser.Username,
                Email = updateUser.Email,
            };
        }
        //Soft delete user
        public async Task<bool> DeactivateUserAsync(int id)
        {
            //Retrieve
            var user = await _context.Users.FindAsync(id);
            //validate
            if (user == null)
                return false;
            //Mutate
            user.IsActive = false;
            //persist
            await _context.SaveChangesAsync();

            return true;
        }
        //Re-Activate user
        public async Task<bool> ReactivateUserAsync(int id)
        {
            //Retrieve
            var user = await _context.Users.FindAsync(id);
            //validate
            if (user == null)
                return false;
            //Mutate
            user.IsActive = true;
            //persist
            await _context.SaveChangesAsync();

            return true;
        }
        //Change Password
        public async Task<GetUserDto?> ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            //retrieve
            var user = await _context.Users
                .FindAsync(userId);
            
            //validate
            if(user == null)
                throw new ArgumentException("User does not exists.");
            //Mutate
            var hasher = new PasswordHasher<UserModel>();

            var result = hasher.VerifyHashedPassword(
                user,
                user.Password,
                dto.CurrentPassword
            );
            //Validate if password fail
            if(result == PasswordVerificationResult.Failed)
                throw new ArgumentException("Current password is incorrect.");
            //validate if the same password
            if(dto.CurrentPassword == dto.NewPassword)
                throw new ArgumentException("New Password must be different from current password.");
            //validate if null or whitespace
            ValidateString(dto.NewPassword);
            //Mutate
            user.Password = hasher.HashPassword(
                user,
                dto.NewPassword
            );
            //Persist
            await _context.SaveChangesAsync();
            return MapToUserDto(user);
        }
        //Delete User
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return false;
            //Mutate
            _context.Users.Remove(user);
            //persist
            await _context.SaveChangesAsync();
            return true;
        }
        //Mapper method
        private static GetUserDto MapToUserDto(UserModel u) => new()
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            IsActive = u.IsActive,
            CreatedDate = u.CreatedDate
        };
        //Validate string method
        public string ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.");
            return value;
        }
    }
}