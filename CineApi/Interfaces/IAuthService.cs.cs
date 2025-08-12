using CineApi.Models.LoginDtos;

namespace CineApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<bool> UserExists(string email);
        Task<UserDto> GetUserById(int id);
    }
}
