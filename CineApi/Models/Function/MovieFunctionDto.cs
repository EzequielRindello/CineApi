using CineApi.Models.Movie;

namespace CineApi.Models.Function
{
    public class MovieFunctionDto
    {
        public required int Id { get; set; }
        public required DateTime Date { get; set; }
        public required TimeSpan Time { get; set; }
        public required decimal Price { get; set; }
        public required int MovieId { get; set; }
        public required int TotalCapacity { get; set; }
        public required int AvailableSeats { get; set; }
        public MovieDto? Movie { get; set; }
    }
}
