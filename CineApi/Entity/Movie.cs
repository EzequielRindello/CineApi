using System.ComponentModel.DataAnnotations;

namespace CineApi.Entity
{
    public class Movie
    {
        public int Id { get; set; }

        [Required][MaxLength(300)] public string Title { get; set; }

        [Required][MaxLength(50)] public string Type { get; set; } // "international" or "national"

        [Required] public string Poster { get; set; }

        [Required][MaxLength(1000)] public string Description { get; set; }

        public int DirectorId { get; set; }

        // Navigation property
        public Director Director { get; set; }

        public ICollection<MovieFunction> MovieFunctions { get; set; }
    }
}
