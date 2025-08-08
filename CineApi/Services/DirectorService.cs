using CineApi.Data;
using CineApi.Entity;
using CineApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorDto>> GetAllDirectorsAsync();
        Task<DirectorDto?> GetDirectorByIdAsync(int id);
    }

    public class DirectorService : IDirectorService
    {
        private readonly AppDbContext _context;

        public DirectorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DirectorDto>> GetAllDirectorsAsync()
        {
            var directors = await _context.Directors.ToListAsync();
            return directors.Select(MapToDto);
        }

        public async Task<DirectorDto?> GetDirectorByIdAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            return director != null ? MapToDto(director) : null;
        }

        private static DirectorDto MapToDto(Director director)
        {
            return new DirectorDto
            {
                Id = director.Id,
                Name = director.Name,
                Nationality = director.Nationality
            };
        }
    }
}