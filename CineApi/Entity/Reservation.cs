using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int MovieFunctionId { get; set; }

        [Required]
        [Range(1, 4)]
        public int TicketQuantity { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public User User { get; set; }
        public MovieFunction MovieFunction { get; set; }
    }
}
