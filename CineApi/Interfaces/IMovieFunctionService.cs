using CineApi.Models.Function;

namespace CineApi.Interfaces
{
    public interface IMovieFunctionService
    {
        Task<IEnumerable<MovieFunctionDto>> GetAllFunctions();
        Task<MovieFunctionDto?> GetFunctionById(int id);
        Task<MovieFunctionDto> CreateFunction(CreateMovieFunctionRequest request);
        Task<MovieFunctionDto?> UpdateFunction(int id, UpdateMovieFunctionRequest request);
        Task<bool> DeleteFunction(int id);
    }

}
