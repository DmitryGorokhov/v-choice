using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using v_choice.Models;
using v_choice.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private IGenreRepository _genres;
        private readonly ILogger _logger;

        public GenresController(IGenreRepository genres, ILogger<GenresController> logger)
        {
            _genres = genres;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Genre> GetAll()
        {
            _logger.LogInformation("Get all genres");
            return _genres.GetAllGenres();
        }

        [HttpGet("{id}")]
        public async Task<ICollection<Film>> GetFilmsByGenreId([FromRoute] int id)
        {
            _logger.LogInformation($"Get films that has genre with Id equal {id}");
            var res = await _genres.GetFilmsByGenreIdAsync(id);
            if (res == null)
            {
                _logger.LogInformation($"Get films by genre id={id}: collection is empty");
                return new HashSet<Film>();
            }
            _logger.LogInformation($"Get films by genre id={id}: collection is found");
            return res;
        }
    }
}
