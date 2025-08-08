using CineApi.Models;
using CineApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MovieDto>> CreateMovie(CreateMovieDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var movie = await _movieService.CreateMovieAsync(request);
                return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating movie", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDto request)
        {
            if (id != request.Id)
                return BadRequest("Movie ID mismatch");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var movie = await _movieService.UpdateMovieAsync(request);
                if (movie == null)
                    return NotFound();
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating movie", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting movie", details = ex.Message });
            }
        }
    }
}