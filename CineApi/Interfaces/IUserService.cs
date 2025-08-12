using CineApi.Models.LoginDtos;

namespace CineApi.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> CreateUser(CreateUserDto createUserDto);
        Task<UserDto> UpdateUser(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUser(int id);
        Task<bool> UserExists(string email);
    }
}