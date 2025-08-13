using CineApi.Models.Director;

namespace CineApi.Interfaces
{
    public interface IDirectorService
    {
        Task<IEnumerable<DirectorDto>> GetAllDirectors();
        Task<DirectorDto?> GetDirectorById(int id);
    }

}
