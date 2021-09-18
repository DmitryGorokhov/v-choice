using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BLL.Interface;
using BLL.DTO;

namespace v_choice.Controllers
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

        [HttpGet("{id}")]
        public async Task<ICollection<FilmDTO>> GetFilmsByGenreId([FromRoute] int id)
        {
            _logger.LogInformation($"Get films that has genre with Id equal {id}");
            var res = await _crudService.GetFilmsByGenreIdAsync(id);
            if (res == null)
            {
                _logger.LogInformation($"Get films by genre id={id}: collection is empty");
                return new HashSet<FilmDTO>();
            }
            _logger.LogInformation($"Get films by genre id={id}: collection is found");
            return res;
        }
    }
}
