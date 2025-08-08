using CineApi.Models;
using CineApi.Models.Consts;
using CineApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CineApi.Services.AuthService;

namespace CineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserForSysAdmin(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var currentUserRole = User.FindFirst("role")?.Value;

                if (currentUserId != id && currentUserRole != "SysAdmin")
                {
                    return Forbid(AuthValidationMessages.OnlyViewOwnProfile());
                }

                var user = await _authService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = AuthValidationMessages.UserNotFound() });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }

        [HttpGet("users")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }

        [HttpPost("users")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.CreateUserAsync(createUserDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }

        [HttpPut("users/{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.UpdateUserAsync(id, updateUserDto);
                if (result == null)
                {
                    return NotFound(new { message = AuthValidationMessages.UserNotFound() });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }

        [HttpDelete("users/{id}")]
        [Authorize(Roles = "SysAdmin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _authService.DeleteUserAsync(id);
                if (!success)
                {
                    return NotFound(new { message = AuthValidationMessages.UserNotFound() });
                }

                return Ok(new { message = AuthValidationMessages.UserDeletedSuccessfully() });
            }
            catch (CannotDeleteSysAdminException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError() });
            }
        }
    }
}