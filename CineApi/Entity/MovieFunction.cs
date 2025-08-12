using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class MovieFunction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required DateTime Date { get; set; }

        public required TimeSpan Time { get; set; }

        public required decimal Price { get; set; }

        [ForeignKey(nameof(Movie))]
        public required int MovieId { get; set; }

        // Navigation property
        public Movie? Movie { get; set; }

        public int TotalCapacity { get; set; } = 50;

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        [NotMapped]
        public int AvailableSeats => TotalCapacity - Reservations.Sum(r => r.TicketQuantity);
    }
}
