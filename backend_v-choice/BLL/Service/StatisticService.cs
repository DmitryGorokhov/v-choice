using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Enum;
using DAL.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class StatisticService : IStatisticService
    {
        private readonly ILogger _logger;
        private readonly IStatisticRepository _statisticRepository;

        public StatisticService(ILogger<StatisticService> logger, IStatisticRepository sr)
        {
            _statisticRepository = sr;
            _logger = logger;
        }

        public Task<ICollection<FilmStatisticDTO>> GetFilmStatisticAsync(FilmStaticticQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralStatisticDTO> GetGeneralStatisticAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<GenreStatisticDTO>> GetGenreStatisticAsync(GenreStaticticQuery query)
        {
            try
            {
                _logger.LogInformation("Starting get genre statistic");
                _logger.LogInformation("Call GetGenreStatisticAsync");

                //IEnumerable<GeneralStatisticDTO> answer = query.SortingType switch
                //{
                //    GenreStatisticSortingType.Films => _statisticRepository.GetGenreStatisticByFilmsAsync(),
                //    GenreStatisticSortingType.Requested => _statisticRepository.GetGenreStatisticByRequestedAsync(),
                //};

                return null;

            }
            catch (Exception e)
            {
                _logger.LogError($"Get genre statistic has thrown an exception: {e.Message}.");
                
                return null;
            }
        }
    }
}
