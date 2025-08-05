using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class MovieFunction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public DateTime Date { get; set; }

        [Required] public TimeSpan Time { get; set; }

        [Required][Column(TypeName = "decimal(18,2)")] public decimal Price { get; set; }

        public int MovieId { get; set; }

        // Navigation property
        public Movie Movie { get; set; }

        [Required]
        public int TotalCapacity { get; set; } = 50;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        [NotMapped]
        public int AvailableSeats => TotalCapacity - Reservations.Sum(r => r.TicketQuantity);
    }
}
