using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : Controller
    {
        private DBContext _context;

        public FilmsController(DBContext context)
        {
            _context = context;
            if (_context.Film.Count() == 0)
            {
                _context.Film.Add(new Film { 
                    Title = "Fake Film",
                    Year = 2021,
                    Description = "Пустое описание",
                });
                    
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Film> GetAll()
        {
            return _context.Film.Include(f => f.Genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var film = await _context.Film.SingleOrDefaultAsync(m => m.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            return Ok(film);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (film.Genres.Count != 0)
            {
                foreach (var genre in film.Genres)
                {
                    var g = _context.Genre.FirstOrDefault(e => e.Id == genre.Id);
                    if (g != null)
                    {
                        g.Films.Add(film);
                        _context.Genre.Update(g);
                    }
                }
            }
            film.Genres = new HashSet<Genre>();
            _context.Film.Add(film);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilm", new { id = film.Id }, film);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _context.Film.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            item.Title = film.Title;
            item.Year = film.Year;
            item.Description = film.Description;
            item.Genres = film.Genres;

            _context.Film.Update(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _context.Film.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            _context.Film.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
