using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Consts.UserRoles;
using CineApi.Models.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        private int CurrentUserId => int.Parse(User.FindFirst("id")?.Value ?? "0");
        private string CurrentUserRole => User.FindFirst("role")?.Value ?? "";

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _reservationService.CreateReservation(request, CurrentUserId);
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
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetMyReservations()
        {
            try
            {
                var reservations = await _reservationService.GetUserReservations(CurrentUserId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.SysAdmin},{UserRoles.CineAdmin}")]
        public async Task<IActionResult> GetAllReservations()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservations();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation([FromRoute] int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationById(id);
                if (reservation == null)
                {
                    return NotFound(new { message = ReservationValidationMessages.ReservationNotFound() });
                }

                // Check if user can access this reservation
                if (reservation.UserId != CurrentUserId &&
                    CurrentUserRole != UserRoles.SysAdmin &&
                    CurrentUserRole != UserRoles.CineAdmin)
                {
                    return Forbid(ReservationValidationMessages.OnlyViewOwnReservations());
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation([FromRoute] int id, [FromBody] UpdateReservationDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _reservationService.UpdateReservation(id, request, CurrentUserId);
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
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError(), details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation([FromRoute] int id)
        {
            try
            {
                var success = await _reservationService.DeleteReservation(id, CurrentUserId);
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
                return StatusCode(500, new { message = ReservationValidationMessages.InternalServerError(), details = ex.Message });
            }
        }
    }
}