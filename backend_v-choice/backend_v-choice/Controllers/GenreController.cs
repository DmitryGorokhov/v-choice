using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;

namespace backend_v_choice.Controllers
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
    }
}
