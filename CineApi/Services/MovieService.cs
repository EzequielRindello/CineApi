using CineApi.Data;
using CineApi.Entity;
using CineApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto?> GetMovieByIdAsync(int id);
    }

    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Director)
                .ToListAsync();

            return movies.Select(MapToDto);
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie != null ? MapToDto(movie) : null;
        }

        private static MovieDto MapToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Type = movie.Type,
                Poster = movie.Poster,
                Description = movie.Description,
                DirectorId = movie.DirectorId,
                Director = movie.Director != null ? new DirectorDto
                {
                    Id = movie.Director.Id,
                    Name = movie.Director.Name,
                    Nationality = movie.Director.Nationality
                } : null
            };
        }
    }
}
