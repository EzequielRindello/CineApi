using System.ComponentModel.DataAnnotations;

namespace CineApi.Models
{
    public class Reservations
    {
        public class ReservationDto
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int MovieFunctionId { get; set; }
            public int TicketQuantity { get; set; }
            public DateTime ReservationDate { get; set; }
            public decimal TotalAmount { get; set; }
            public UserDto? User { get; set; }
            public MovieFunctionDto? MovieFunction { get; set; }
        }

        public class CreateReservationDto
        {
            [Required]
            public int MovieFunctionId { get; set; }

            [Required]
            [Range(1, 4, ErrorMessage = "Ticket quantity must be between 1 and 4")]
            public int TicketQuantity { get; set; }
        }

        public class UpdateReservationDto
        {
            [Required]
            public int MovieFunctionId { get; set; }
            [Required]
            public int TicketQuantity { get; set; }
            [Required]
            public DateTime ReservationDate { get; set; }
            [Required]
            public decimal TotalAmount { get; set; }
            public UserDto? User { get; set; }
            public MovieFunctionDto? MovieFunction { get; set; }
        }
    }
}
