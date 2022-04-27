using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly ILogger _logger;

        public StatisticController(IStatisticService ss, ILogger<StatisticController> logger)
        {
            _statisticService = ss;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetGeneralStatistic()
        {
            _logger.LogInformation("Get general statistic");
            GeneralStatistic res = _statisticService.GetGeneralStatistic();

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilmStatistic([FromBody] FilmStaticticQuery query)
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

        [HttpGet]
        public async Task<IActionResult> GetGenreStatistic([FromBody] GenreStaticticQuery query)
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
    }
}
