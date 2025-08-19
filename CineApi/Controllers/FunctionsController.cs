using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Function;
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
            var functions = await _movieFunctionService.GetAllFunctions();
            return Ok(functions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieFunctionDto>> GetFunctionById([FromRoute] int id)
        {
            var function = await _movieFunctionService.GetFunctionById(id);
            if (function == null)
                return NotFound();
            return Ok(function);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MovieFunctionDto>> CreateFunction([FromBody] CreateMovieFunctionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var function = await _movieFunctionService.CreateFunction(request);
                return CreatedAtAction(nameof(GetFunctionById), new { id = function.Id }, function);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = FunctionValidationMessages.ErrorCreatingFunction(), details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<MovieFunctionDto>> UpdateFunction([FromRoute] int id, [FromBody] UpdateMovieFunctionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var function = await _movieFunctionService.UpdateFunction(id, request);
                if (function == null)
                    return NotFound();
                return Ok(function);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = FunctionValidationMessages.ErrorUpdatingFunction(), details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteFunction([FromRoute] int id)
        {
            var deleted = await _movieFunctionService.DeleteFunction(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}