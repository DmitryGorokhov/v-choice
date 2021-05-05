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
    public class UserController : Controller
    {
        private readonly IUserRepository _users;
        private readonly ILogger _logger;

        public UserController(IUserRepository users, ILogger<UserController> logger)
        {
            _users = users;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Film>> GetFavoriteFilmsAsync()
        {
            _logger.LogInformation("Get favorite films for authorized user.");
            return await _users.GetAllFavoriteFilmsAsync(HttpContext.User);
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
            var res = await _users.CheckFilmIsAdded(id, HttpContext.User);
            if (res == null)
            {
                _logger.LogInformation($"Check film in favorites: film with Id equal {id} not found.");
                return NotFound(id);
            }
            _logger.LogInformation($"Check film in favorites: Ok - {res}.");
            return Ok(res);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> AddFilm([FromBody] Film film)
        {
            _logger.LogInformation($"Add film to favorite films of authorized user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Add film in favorites: model state is not valid.");
                return BadRequest(ModelState);
            }
            await _users.AddFavoriteFilmAsync(film, HttpContext.User);
            _logger.LogInformation($"Add film in favorites: film was added to favorite films.");
            return NoContent();
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteFilm([FromBody] Film film)
        {
            _logger.LogInformation($"Delete film from favorite films of authorized user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete film from favorites: model state is not valid.");
                return BadRequest(ModelState);
            }
            try
            {
                await _users.RemoveFilmFromFavorite(film, HttpContext.User);
                _logger.LogInformation($"Delete film from favorites: film was deleted from favorite films.");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film from favorites: {e.Message}.");
                return NoContent();
            }
        }
    }
}
