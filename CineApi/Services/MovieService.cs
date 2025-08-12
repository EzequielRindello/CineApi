using CineApi.Data;
using CineApi.Entity;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Movie;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDto>> GetAllMovies()
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Include(m => m.Director)
                .ToListAsync();

            return movies.Select(DataMapper.MapToMovieDto);
        }

        public async Task<MovieDto?> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .AsNoTracking()
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie != null ? DataMapper.MapToMovieDto(movie) : null;
        }

        public async Task<MovieDto> CreateMovie(CreateMovieDto request)
        {

            var directorExists = await _context.Directors.AnyAsync(d => d.Id == request.DirectorId);
            if (!directorExists)
                throw new ArgumentException(MovieValidationMessage.DirectorNotfound());

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

            var createdMovie = await GetMovieById(movie.Id);
            return createdMovie!;
        }

        public async Task<MovieDto?> UpdateMovie(UpdateMovieDto request)
        {
            var movie = await _context.Movies.FindAsync(request.Id);
            if (movie == null)
                return null;

            var directorExists = await _context.Directors.AnyAsync(d => d.Id == request.DirectorId);
            if (!directorExists)
                throw new ArgumentException(MovieValidationMessage.DirectorNotfound());

            movie.Title = request.Title;
            movie.Type = request.Type;
            movie.Poster = request.Poster;
            movie.Description = request.Description;
            movie.DirectorId = request.DirectorId;

            await _context.SaveChangesAsync();

            return await GetMovieById(request.Id);
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
