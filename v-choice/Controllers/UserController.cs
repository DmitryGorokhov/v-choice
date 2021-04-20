using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using v_choice.Models;
using v_choice.Interfaces;
using System;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _users;

        public UserController(IUserRepository users)
        {
            _users = users;
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Film>> GetFavoriteFilmsAsync()
        {
            return await _users.GetAllFavoriteFilmsAsync(HttpContext.User);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> AddFilm([FromBody] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _users.AddFavoriteFilmAsync(film, HttpContext.User);
            return NoContent();
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteFilm([FromBody] Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _users.RemoveFilmFromFavorite(film, HttpContext.User);
                return NoContent();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }
    }
}
