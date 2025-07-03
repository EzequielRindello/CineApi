using System.ComponentModel.DataAnnotations;

namespace CineApi.Models
{
    public class MovieFunctionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public decimal Price { get; set; }
        public int MovieId { get; set; }
        public MovieDto? Movie { get; set; }
    }

    public class CreateMovieFunctionRequest
    {
        [Required] public DateTime Date { get; set; }

        [Required] public TimeSpan Time { get; set; }

        [Required][Range(0, double.MaxValue, ErrorMessage = "Price must be positive")] public decimal Price { get; set; }

        [Required] public int MovieId { get; set; }
    }

    public class UpdateMovieFunctionRequest
    {
        [Required] public DateTime Date { get; set; }

        [Required] public TimeSpan Time { get; set; }

        [Required][Range(0, double.MaxValue, ErrorMessage = "Price must be positive")] public decimal Price { get; set; }

        [Required] public int MovieId { get; set; }
    }
}