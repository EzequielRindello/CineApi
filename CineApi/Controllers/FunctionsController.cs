using CineApi.Models;
using CineApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunctionsController : ControllerBase
    {
        private readonly IMovieFunctionService _movieFunctionService;

        public FunctionsController(IMovieFunctionService movieFunctionService)
        {
            _movieFunctionService = movieFunctionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieFunctionDto>>> GetAllFunctions()
        {
            var functions = await _movieFunctionService.GetAllFunctionsAsync();
            return Ok(functions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieFunctionDto>> GetFunctionById(int id)
        {
            var function = await _movieFunctionService.GetFunctionByIdAsync(id);
            if (function == null)
                return NotFound();

            return Ok(function);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MovieFunctionDto>> CreateFunction(CreateMovieFunctionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var function = await _movieFunctionService.CreateFunctionAsync(request);
                return CreatedAtAction(nameof(GetFunctionById), new { id = function.Id }, function);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating function", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<MovieFunctionDto>> UpdateFunction(int id, UpdateMovieFunctionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var function = await _movieFunctionService.UpdateFunctionAsync(id, request);
                if (function == null)
                    return NotFound();

                return Ok(function);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating function", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteFunction(int id)
        {
            var deleted = await _movieFunctionService.DeleteFunctionAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
