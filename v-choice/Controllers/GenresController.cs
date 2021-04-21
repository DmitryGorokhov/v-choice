using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using v_choice.Models;
using v_choice.Interfaces;
using System.Threading.Tasks;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private IGenreRepository _genres;
        public GenresController(IGenreRepository genres)
        {
            _genres = genres;
        }

        [HttpGet]
        public IEnumerable<Genre> GetAll()
        {
            return _genres.GetAllGenres();
        }

        [HttpGet("{id}")]
        public async Task<ICollection<Film>> GetFilmsByGenreId([FromRoute] int id)
        {
            var res = await _genres.GetFilmsByGenreIdAsync(id);
            if (res == null)
                return new HashSet<Film>();
            return res;
        }
    }
}
