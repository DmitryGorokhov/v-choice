using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using BLL.Interface;
using BLL.DTO;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger _logger;

        public FavoriteController(IFavoriteService fs, ILogger<FavoriteController> logger)
        {
            _favoriteService = fs;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<FilmDTO>> GetFavoriteFilmsAsync()
        {
            _logger.LogInformation("Get favorite films for authorized user.");
            return await _favoriteService.GetAllFavoriteFilmsAsync(HttpContext.User);
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
