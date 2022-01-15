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
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IPaginationService _paginationService;
        private readonly ILogger _logger;

        public FavoriteController(IFavoriteService fs, IPaginationService ps, ILogger<FavoriteController> logger)
        {
            _favoriteService = fs;
            _paginationService = ps;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFavoriteFilmsPagination([FromQuery] PaginationQuery query)
        {
            _logger.LogInformation("Get pagination favorite films for authorized user.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get pagination favorite films for authorized user: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            var res = await _paginationService.GetFilmsPagination(query);
            if (res == null) return StatusCode(500);

            return Ok(res);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> CheckFilmIsAdded([FromRoute] int id)
        {
            _logger.LogInformation($"Check film with Id equal {id} is add to favorite films of authorized user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Check film in favorites: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            var res = await _favoriteService.CheckFilmIsAdded(id, HttpContext.User);
            if (res == null) return NotFound(id);

            return Ok(res);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> AddFilm([FromBody] FilmDTO film)
        {
            _logger.LogInformation($"Add film to favorite films of authorized user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Add film in favorites: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            await _favoriteService.AddFavoriteFilmAsync(film, HttpContext.User);
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteFilm([FromBody] FilmDTO film)
        {
            _logger.LogInformation($"Delete film from favorite films of authorized user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete film from favorites: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            await _favoriteService.RemoveFilmFromFavorite(film, HttpContext.User);
            return NoContent();
        }
    }
}
