using CineApi.Data;
using CineApi.Entity;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.LoginDtos;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CineApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException(AuthValidationMessages.InvalidCredentials());
            }

            var token = JwtHelper.GenerateJwtToken(user, _configuration);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            };
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email))
            {
                throw new InvalidOperationException(AuthValidationMessages.UserAlreadyExists());
            }

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
                Role = UserRole.User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = JwtHelper.GenerateJwtToken(user, _configuration);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            };
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
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
            if (await UserExists(createUserDto.Email))
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

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

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