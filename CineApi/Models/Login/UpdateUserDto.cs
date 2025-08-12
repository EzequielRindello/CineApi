using CineApi.Entity;

namespace CineApi.Models.LoginDtos
{
    public class UpdateUserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
