using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using v_choice.Models;
using v_choice.Interfaces;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : Controller
    {
        private IFilmRepository _films;

        public FilmsController(IFilmRepository films)
        {
            _films = films;
        }

        [HttpGet]
        public IEnumerable<Film> GetAll()
        {
            return (IEnumerable<Film>)_films.GetAllFilmsAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var film = await _films.GetFilmAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return Ok(film);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _films.CreateFilmAsync(film);
            return CreatedAtAction("GetFilm", new { id = film.Id }, film);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _films.UpdateFilmAsync(id, film);
                return NoContent();
            }
            catch (System.Exception)
            {
                return NoContent();
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _films.DeleteFilmAsync(id);
                return NoContent();
            }
            catch (System.Exception)
            {
                return NoContent();
            }
        }
    }
}
