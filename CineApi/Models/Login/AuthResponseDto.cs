namespace CineApi.Models.LoginDtos
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public UserDto? User { get; set; }
    }
}
