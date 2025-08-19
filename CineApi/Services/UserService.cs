using CineApi.Data;
using CineApi.Entity;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.LoginDtos;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.ToString()
            }).ToList();
        }

        public async Task<UserDto> CreateUser(CreateUserDto createUserDto)
        {
            if (await AuthHelper.UserExists(createUserDto.Email, _context))
            {
                throw new InvalidOperationException(AuthValidationMessages.UserAlreadyExists());
            }

            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                PasswordHash = PasswordHelper.HashPassword(createUserDto.Password),
                Role = createUserDto.Role ?? UserRole.User,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<UserDto> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            // Single query to get both the user and check for email conflicts
            var users = await _context.Users
                .Where(u => u.Id == id || u.Email == updateUserDto.Email)
                .ToListAsync();

            var user = users.FirstOrDefault(u => u.Id == id) ??
                throw new InvalidOperationException(AuthValidationMessages.UserNotFound());

            var emailConflict = users.Any(u => u.Email == updateUserDto.Email && u.Id != id);
            if (emailConflict)
            {
                throw new InvalidOperationException(AuthValidationMessages.EmailAlreadyUsed());
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Email = updateUserDto.Email;
            user.Role = updateUserDto.Role ?? UserRole.User;

            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}