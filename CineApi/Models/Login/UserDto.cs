﻿namespace CineApi.Models.LoginDtos
{
    public class UserDto
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}
