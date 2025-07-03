using CineApi.Data;
using CineApi.Entity;
using CineApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public interface IMovieFunctionService
    {
        Task<IEnumerable<MovieFunctionDto>> GetAllFunctionsAsync();
        Task<MovieFunctionDto?> GetFunctionByIdAsync(int id);
        Task<MovieFunctionDto> CreateFunctionAsync(CreateMovieFunctionRequest request);
        Task<MovieFunctionDto?> UpdateFunctionAsync(int id, UpdateMovieFunctionRequest request);
        Task<bool> DeleteFunctionAsync(int id);
    }

    public class MovieFunctionService : IMovieFunctionService
    {
        private readonly AppDbContext _context;

        public MovieFunctionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieFunctionDto>> GetAllFunctionsAsync()
        {
            var functions = await _context.MovieFunctions
                .Include(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .ToListAsync();

            return functions.Select(MapToDto);
        }

        public async Task<MovieFunctionDto?> GetFunctionByIdAsync(int id)
        {
            var function = await _context.MovieFunctions
                .Include(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .FirstOrDefaultAsync(mf => mf.Id == id);

            return function != null ? MapToDto(function) : null;
        }

        public async Task<MovieFunctionDto> CreateFunctionAsync(CreateMovieFunctionRequest request)
        {
            var function = new MovieFunction
            {
                Date = request.Date,
                Time = request.Time,
                Price = request.Price,
                MovieId = request.MovieId
            };

            _context.MovieFunctions.Add(function);
            await _context.SaveChangesAsync();

            return await GetFunctionByIdAsync(function.Id);
        }

        public async Task<MovieFunctionDto?> UpdateFunctionAsync(int id, UpdateMovieFunctionRequest request)
        {
            var function = await _context.MovieFunctions.FindAsync(id);
            if (function == null)
                return null;

            function.Date = request.Date;
            function.Time = request.Time;
            function.Price = request.Price;
            function.MovieId = request.MovieId;

            await _context.SaveChangesAsync();

            return await GetFunctionByIdAsync(id);
        }

        public async Task<bool> DeleteFunctionAsync(int id)
        {
            var function = await _context.MovieFunctions.FindAsync(id);
            if (function == null)
                return false;

            _context.MovieFunctions.Remove(function);
            await _context.SaveChangesAsync();
            return true;
        }

        private static MovieFunctionDto MapToDto(MovieFunction function)
        {
            return new MovieFunctionDto
            {
                Id = function.Id,
                Date = function.Date,
                Time = function.Time,
                Price = function.Price,
                MovieId = function.MovieId,
                Movie = function.Movie != null ? new MovieDto
                {
                    Id = function.Movie.Id,
                    Title = function.Movie.Title,
                    Type = function.Movie.Type,
                    Poster = function.Movie.Poster,
                    Description = function.Movie.Description,
                    DirectorId = function.Movie.DirectorId,
                    Director = function.Movie.Director != null ? new DirectorDto
                    {
                        Id = function.Movie.Director.Id,
                        Name = function.Movie.Director.Name,
                        Nationality = function.Movie.Director.Nationality
                    } : null
                } : null
            };
        }

    }
}
