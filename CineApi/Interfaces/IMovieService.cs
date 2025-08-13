using CineApi.Models.Movie;

namespace CineApi.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMovies();
        Task<MovieDto?> GetMovieById(int id);
        Task<MovieDto> CreateMovie(CreateMovieDto request);
        Task<MovieDto?> UpdateMovie(UpdateMovieDto request);
        Task<bool> DeleteMovie(int id);
    }
}
