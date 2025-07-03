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
}
