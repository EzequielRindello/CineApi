using CineApi.Models.Function;
using CineApi.Models.LoginDtos;

namespace CineApi.Models.Reservation
{
    public class UpdateReservationDto
    {
        public required int MovieFunctionId { get; set; }
        public required int TicketQuantity { get; set; }
        public required DateTime ReservationDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public UserDto? User { get; set; }
        public MovieFunctionDto? MovieFunction { get; set; }
    }
}
