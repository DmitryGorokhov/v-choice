using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Enum;
using DAL.Interface;
using DAL.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class StatisticService : IStatisticService
    {
        private readonly ILogger _logger;
        private readonly IStatisticRepository _statisticRepository;
        private readonly IPaginationRepository _paginationRepository;
        private readonly IMapper _mapper;

        public StatisticService(ILogger<StatisticService> logger, IStatisticRepository sr, IPaginationRepository pr, IMapper mapper)
        {
            _statisticRepository = sr;
            _paginationRepository = pr;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationDTO<FilmStatisticDTO>> GetFilmStatisticAsync(FilmStaticticQuery query)
        {
            try
            {
                _logger.LogInformation("Starting get film statistic");
                _logger.LogInformation("Call GetFilmStatistic method");

                IQueryable<Film> answer = query.SortingType switch
                {
                    FilmStatisticSortingType.Requested => _statisticRepository.GetFilmStatisticByRequested(),
                    FilmStatisticSortingType.Rate => _statisticRepository.GetFilmStatisticByRate(),
                    FilmStatisticSortingType.CountRate => _statisticRepository.GetFilmStatisticByCountRate(),
                    FilmStatisticSortingType.Comments => _statisticRepository.GetFilmStatisticByComments(),
                    FilmStatisticSortingType.Favorites => _statisticRepository.GetFilmStatisticByFavorites(),
                    _ => _statisticRepository.GetFilmStatisticByRequested()
                };

                _logger.LogInformation("Call SplitByPagesAsync");
                (int total, IQueryable<Film> items) = await _paginationRepository.SplitByPagesAsync(answer, query.PageNumber, query.OnPageCount);

                _logger.LogInformation("Get film statistic has been done. Prepare DTO to return.");

                return new PaginationDTO<FilmStatisticDTO>(query)
                {
                    TotalCount = total,
                    Items = items.Select(e => _mapper.FilmModelToStatisticDTO(e)).ToList(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Get film statistic has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public GeneralStatistic GetGeneralStatistic()
        {
            try
            {
                _logger.LogInformation("Starting get general statistic");
                _logger.LogInformation("Call GetGeneralStatisticAsync");

                GeneralStatistic res = _statisticRepository.GetGeneralStatistic();

                _logger.LogInformation("Get general statistic has been done.");

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get general statistic has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<PaginationDTO<GenreStatisticDTO>> GetGenreStatisticAsync(GenreStaticticQuery query)
        {
            try
            {
                _logger.LogInformation("Starting get genre statistic");
                _logger.LogInformation("Call GetGenreStatistic method");

                IQueryable<Genre> answer = query.SortingType switch
                {
                    GenreStatisticSortingType.Films => _statisticRepository.GetGenreStatisticByFilms(),
                    GenreStatisticSortingType.Requested => _statisticRepository.GetGenreStatisticByRequested(),
                    _ => _statisticRepository.GetGenreStatisticByFilms()
                };

                _logger.LogInformation("Call SplitByPagesAsync");
                (int total, IQueryable<Genre> items) = await _paginationRepository.SplitByPagesAsync(answer, query.PageNumber, query.OnPageCount);

                _logger.LogInformation("Get genre statistic has been done. Prepare DTO to return.");

                return new PaginationDTO<GenreStatisticDTO>(query)
                {
                    TotalCount = total,
                    Items = items.Select(e => _mapper.GenreModelToStatisticDTO(e)).ToList(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Get genre statistic has thrown an exception: {e.Message}.");

                return null;
            }
        }
    }
}
