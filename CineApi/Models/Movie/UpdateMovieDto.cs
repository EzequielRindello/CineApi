namespace CineApi.Models.Movie
{
    public class UpdateMovieDto
    {

        public required string Title { get; set; }
        public required string Type { get; set; }
        public required string Poster { get; set; }
        public required string Description { get; set; }
        public required int DirectorId { get; set; }
        public required int Id { get; set; }
    }
}
