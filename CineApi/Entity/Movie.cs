using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineApi.Entity
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Type { get; set; } // "international" or "national"

        public required string Poster { get; set; }

        public required string Description { get; set; }

        public required int DirectorId { get; set; }

        // Navigation property
        public Director? Director { get; set; }

        public List<MovieFunction>? MovieFunctions { get; set; }
    }
}
