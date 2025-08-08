using CineApi.Models.Consts;
using CineApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CineApi.Models.Reservations;

namespace CineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized(ReservationValidationMessages.InvalidToken());
                }

                var result = await _reservationService.CreateReservationAsync(request, userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError() });
            }
        }

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetMyReservations()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized(ReservationValidationMessages.InvalidToken());
                }

                var reservations = await _reservationService.GetUserReservationsAsync(userId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError() });
            }
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin,CineAdmin")]
        public async Task<IActionResult> GetAllReservations()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservationsAsync();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError() });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var userRole = User.FindFirst("role")?.Value;

                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return NotFound(new { message = ReservationValidationMessages.ReservationNotFound() });
                }

                if (reservation.UserId != userId && userRole != "SysAdmin" && userRole != "CineAdmin")
                {
                    return Forbid(ReservationValidationMessages.OnlyViewOwnReservations());
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError() });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, UpdateReservationDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized(ReservationValidationMessages.InvalidToken());
                }

                var result = await _reservationService.UpdateReservationAsync(id, request, userId);
                if (result == null)
                {
                    return NotFound(new { message = ReservationValidationMessages.ReservationNotFound() });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError() });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                if (userId == 0)
                {
                    return Unauthorized(ReservationValidationMessages.InvalidToken());
                }

                var success = await _reservationService.CancelReservationAsync(id, userId);
                if (!success)
                {
                    return NotFound(new { message = ReservationValidationMessages.ReservationNotFound() });
                }

                return Ok(new { message = ReservationValidationMessages.ReservationCancelledSuccessfully() });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError() });
            }
        }
    }
}