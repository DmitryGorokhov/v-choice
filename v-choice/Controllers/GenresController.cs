using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private DBContext _context;
        public GenresController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Genre> GetAll()
        {
            return _context.Genre;
        }
    }
}
