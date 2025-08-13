using CineApi.Data;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Director;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly AppDbContext _context;

        public DirectorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DirectorDto>> GetAllDirectors()
        {
            var directors = await _context.Directors
                .AsNoTracking()
                .ToListAsync();

            return directors.Select(DataMapper.MapToDirectorDto);
        }

        public async Task<DirectorDto?> GetDirectorById(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            return director != null ? DataMapper.MapToDirectorDto(director) : null;
        }
    }
}