using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Enum;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : Controller
    {
        private readonly IStatisticService _statisticService;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _appEnvironment;

        public StatisticController(IStatisticService ss, ILogger<StatisticController> logger, IWebHostEnvironment ae)
        {
            _statisticService = ss;
            _logger = logger;
            _appEnvironment = ae;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetGeneralStatistic()
        {
            _logger.LogInformation("Get general statistic");
            GeneralStatistic res = _statisticService.GetGeneralStatistic();

            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [Route("film")]
        [HttpGet]
        public async Task<IActionResult> GetFilmStatistic([FromQuery] FilmStaticticQuery query)
        {
            _logger.LogInformation("Get film statistic");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get film statistic: model state is not valid.");

                return BadRequest(ModelState);
            }

            PaginationDTO<FilmStatisticDTO> res = await _statisticService.GetFilmStatisticAsync(query);

            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [Route("genre")]
        [HttpGet]
        public async Task<IActionResult> GetGenreStatistic([FromQuery] GenreStaticticQuery query)
        {
            _logger.LogInformation("Get genre statistic");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get genre statistic: model state is not valid.");

                return BadRequest(ModelState);
            }

            PaginationDTO<GenreStatisticDTO> res = await _statisticService.GetGenreStatisticAsync(query);

            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult ExportStatistic([FromBody] ExportStatisticQuery exportStatisticQuery)
        {
            _logger.LogInformation("Export statistic");

            string link = _statisticService.ExportStatisticAsync(exportStatisticQuery, _appEnvironment);

            return Ok(new { link = link });
        }
    }
}
