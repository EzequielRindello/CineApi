using CineApi.Data;
using CineApi.Entity;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Function;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class MovieFunctionService : IMovieFunctionService
    {
        private readonly AppDbContext _context;

        public MovieFunctionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieFunctionDto>> GetAllFunctions()
        {
            var functions = await _context.MovieFunctions
                .AsNoTracking()
                .Include(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .Include(mf => mf.Reservations)
                .AsSplitQuery()
                .ToListAsync();

            return functions.Select(DataMapper.MapToMovieDto);
        }

        public async Task<MovieFunctionDto?> GetFunctionById(int id)
        {
            var function = await _context.MovieFunctions
                .AsNoTracking()
                .Include(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .Include(mf => mf.Reservations)
                .AsSplitQuery()
                .FirstOrDefaultAsync(mf => mf.Id == id);

            return function != null ? DataMapper.MapToMovieDto(function) : null;
        }

        public async Task<MovieFunctionDto> CreateFunction(CreateMovieFunctionRequest request)
        {
            var movieExists = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.MovieId) ?? throw new ArgumentException(MovieValidationMessage.MovieNotfound());
            var function = new MovieFunction
            {
                Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
                Time = request.Time,
                Price = request.Price,
                MovieId = request.MovieId,
                TotalCapacity = request.TotalCapacity
            };

            _context.MovieFunctions.Add(function);
            await _context.SaveChangesAsync();

            var createdFunction = await GetFunctionById(function.Id);
            return createdFunction!;
        }

        public async Task<MovieFunctionDto?> UpdateFunction(int id, UpdateMovieFunctionRequest request)
        {
            var function = await _context.MovieFunctions.FindAsync(id);
            if (function == null)
                return null;

            var movieExists = await _context.Movies.AnyAsync(m => m.Id == request.MovieId);
            if (!movieExists)
            {
                throw new ArgumentException(MovieValidationMessage.MovieNotfound());
            }

            function.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);
            function.Time = request.Time;
            function.Price = request.Price;
            function.MovieId = request.MovieId;

            await _context.SaveChangesAsync();

            return await GetFunctionById(id);
        }

        public async Task<bool> DeleteFunction(int id)
        {
            var function = await _context.MovieFunctions.FindAsync(id);
            if (function == null)
                return false;

            _context.MovieFunctions.Remove(function);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}