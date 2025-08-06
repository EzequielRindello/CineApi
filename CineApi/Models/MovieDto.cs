using System.ComponentModel.DataAnnotations;

namespace CineApi.Models
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public string Description { get; set; }
        public int DirectorId { get; set; }
        public DirectorDto? Director { get; set; }
    }

    public class DirectorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
    }

    public class CreateMovieDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public string Poster { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int DirectorId { get; set; }
    }

    public class UpdateMovieDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public string Poster { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int DirectorId { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
