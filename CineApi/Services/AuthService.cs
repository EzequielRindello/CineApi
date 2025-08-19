using CineApi.Data;
using CineApi.Entity;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.LoginDtos;
using Microsoft.EntityFrameworkCore;

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
            if (await AuthHelper.UserExists(registerDto.Email, _context))
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

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new InvalidOperationException(AuthValidationMessages.UserNotFound());

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}