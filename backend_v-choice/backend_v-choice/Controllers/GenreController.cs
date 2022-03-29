using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly ILogger _logger;

        public GenreController(ICrudService cs, ILogger<GenreController> logger)
        {
            _crudService = cs;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<GenreDTO> GetAll()
        {
            _logger.LogInformation("Get all genres");

            return _crudService.GetAllGenres();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateGenreAsync([FromBody] GenreDTO genre)
        {
            _logger.LogInformation("Create genre.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create genre: model state is not valid.");

                return BadRequest(ModelState);
            }

            var genreDTO = await _crudService.CreateGenreAsync(genre);
            if (genreDTO == null) return StatusCode(500);

            return CreatedAtAction("CreateGenre", new { id = genreDTO.Id }, genreDTO);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre([FromRoute] int id, [FromBody] GenreDTO genre)
        {
            _logger.LogInformation($"Update genre with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update genre: model state is not valid.");

                return BadRequest(ModelState);
            }

            await _crudService.UpdateGenreAsync(id, genre);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre([FromRoute] int id)
        {
            _logger.LogInformation($"Delete genre with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete genre: model state is not valid.");

                return BadRequest(ModelState);
            }

            await _crudService.DeleteGenreAsync(id);

            return NoContent();
        }
    }
}
