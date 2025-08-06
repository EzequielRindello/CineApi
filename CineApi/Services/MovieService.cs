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
        Task<MovieDto> CreateMovieAsync(CreateMovieDto request);
        Task<MovieDto> UpdateMovieAsync(UpdateMovieDto request);
        Task DeleteMovieAsync(int id);
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

        public async Task<MovieDto> CreateMovieAsync(CreateMovieDto request)
        {
            var movie = new Movie
            {
                Title = request.Title,
                Type = request.Type,
                Poster = request.Poster,
                Description = request.Description,
                DirectorId = request.DirectorId
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return MapToDto(movie);
        }

        public async Task<MovieDto> UpdateMovieAsync(UpdateMovieDto request)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == request.Id);
            if (movie == null)
                throw new KeyNotFoundException("Movie not found");

            movie.Title = request.Title;
            movie.Type = request.Type;
            movie.Poster = request.Poster;
            movie.Description = request.Description;
            movie.DirectorId = request.DirectorId;

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return MapToDto(movie);
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                throw new KeyNotFoundException("Movie not found");
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
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
