using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using v_choice.Models;
using v_choice.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFilmRepository _films;

        public FilmsController(IFilmRepository films, ILogger<FilmsController> logger)
        {
            _films = films;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Film> GetAll()
        {
            _logger.LogInformation("Get all films");
            return _films.GetAllFilmsAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm([FromRoute] int id)
        {
            _logger.LogInformation($"Get film with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get film: model state is not valid.");
                return BadRequest(ModelState);
            }
            var film = await _films.GetFilmAsync(id);
            if (film == null)
            {
                _logger.LogInformation($"Get film: film with Id equal {id} not found.");
                return NotFound();
            }
            _logger.LogInformation($"Get film: film with Id equal {id} was found.");
            return Ok(film);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Film film)
        {
            _logger.LogInformation("Create film.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create film: model state is not valid.");
                return BadRequest(ModelState);
            }
            await _films.CreateFilmAsync(film);
            _logger.LogInformation($"Create film: film with Id equal {film.Id} was created.");
            return CreatedAtAction("GetFilm", new { id = film.Id }, film);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Film film)
        {
            _logger.LogInformation($"Update film with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update film: model state is not valid.");
                return BadRequest(ModelState);
            }
            try
            {
                await _films.UpdateFilmAsync(id, film);
                _logger.LogInformation($"Update film: film with Id equal {id} was updated.");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Update film with id={id}: {e.Message}.");
                return NoContent();
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation($"Delete film with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete film: model state is not valid.");
                return BadRequest(ModelState);
            }
            try
            {
                await _films.DeleteFilmAsync(id);
                _logger.LogInformation($"Delete film: film with Id equal {id} was deleted.");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film with id={id}: {e.Message}.");
                return NoContent();
            }
        }
    }
}
