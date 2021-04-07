using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using v_choice.Models;
using v_choice.Interfaces;

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
    }
}
