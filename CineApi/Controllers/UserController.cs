using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Consts.UserRoles;
using CineApi.Models.LoginDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.SysAdmin)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.SysAdmin)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.CreateUser(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.SysAdmin)]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.UpdateUser(id, request);
                if (result == null)
                {
                    return NotFound(new { message = AuthValidationMessages.UserNotFound() });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.SysAdmin)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var success = await _userService.DeleteUser(id);
                if (!success)
                {
                    return NotFound(new { message = AuthValidationMessages.UserNotFound() });
                }

                return Ok(new { message = AuthValidationMessages.UserDeletedSuccessfully() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = AuthValidationMessages.InternalServerError(), details = ex.Message });
            }
        }
    }
}