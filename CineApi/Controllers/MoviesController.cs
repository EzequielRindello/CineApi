using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Movie;
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
            var movies = await _movieService.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovieById([FromRoute] int id)
        {
            var movie = await _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MovieDto>> CreateMovie([FromBody] CreateMovieDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var movie = await _movieService.CreateMovie(request);
                return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = MovieValidationMessage.ErrorCreatingMovie(), details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, UpdateMovieDto request)
        {
            if (id != request.Id)
                return BadRequest(MovieValidationMessage.MovieIdMismatch());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var movie = await _movieService.UpdateMovie(request);
                if (movie == null)
                    return NotFound();
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = MovieValidationMessage.ErrorUpdatingMovie(), details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            try
            {
                await _movieService.DeleteMovie(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = MovieValidationMessage.ErrorDeletingMovie(), details = ex.Message });
            }
        }
    }
}