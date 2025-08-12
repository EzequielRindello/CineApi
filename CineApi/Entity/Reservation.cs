using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }


        [ForeignKey(nameof(MovieFunction))]
        public required int MovieFunctionId { get; set; }

        public required int TicketQuantity { get; set; }


        public required DateTime ReservationDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public MovieFunction? MovieFunction { get; set; }
    }
}
