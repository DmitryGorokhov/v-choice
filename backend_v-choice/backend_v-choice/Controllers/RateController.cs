using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly ILogger _logger;

        public RateController(ICrudService cs, IPaginationService ps, ILogger<CommentController> logger)
        {
            _crudService = cs;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("{filmId}")]
        public async Task<IActionResult> GetFilmRate([FromRoute] int filmId)
        {
            _logger.LogInformation("Get rate of film of current user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get rate of film of current user: model state is not valid.");

                return BadRequest(ModelState);
            }

            var res = await _crudService.GetFilmRate(filmId, HttpContext.User);
            
            return Ok(res);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRate([FromBody] RateDTO rate)
        {
            _logger.LogInformation("Create rate.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create rate: model state is not valid.");

                return BadRequest(ModelState);
            }

            var rateDTO = await _crudService.CreateRateAsync(rate, HttpContext.User);
            if (rateDTO == null) return StatusCode(500);

            return CreatedAtAction("CreateRate", new { id = rate.Id }, rate);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRate([FromRoute] int id, [FromBody] RateDTO rate)
        {
            _logger.LogInformation($"Update rate with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update rate: model state is not valid.");
    
                return BadRequest(ModelState);
            }

            await _crudService.UpdateRateAsync(id, rate);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate([FromRoute] int id)
        {
            _logger.LogInformation($"Delete rate with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete rate: model state is not valid.");

                return BadRequest(ModelState);
            }

            await _crudService.DeleteRateAsync(id);

            return NoContent();
        }
    }
}
