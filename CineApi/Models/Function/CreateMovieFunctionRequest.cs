namespace CineApi.Models.Function
{
    public class CreateMovieFunctionRequest
    {
        public required DateTime Date { get; set; }
        public required TimeSpan Time { get; set; }
        public required decimal Price { get; set; }
        public required int MovieId { get; set; }
        public required int TotalCapacity { get; set; } = 50;
    }
}
