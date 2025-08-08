using CineApi.Models;
using CineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorDto>>> GetAllDirectors()
        {
            var directors = await _directorService.GetAllDirectorsAsync();
            return Ok(directors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorDto>> GetDirectorById(int id)
        {
            var director = await _directorService.GetDirectorByIdAsync(id);
            if (director == null)
                return NotFound();

            return Ok(director);
        }
    }
}