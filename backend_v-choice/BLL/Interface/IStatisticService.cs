using BLL.DTO;
using BLL.Query;
using DAL.Enum;
using DAL.Model;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IStatisticService
    {
        GeneralStatistic GetGeneralStatistic();
        Task<PaginationDTO<FilmStatisticDTO>> GetFilmStatisticAsync(FilmStaticticQuery query);
        Task<PaginationDTO<GenreStatisticDTO>> GetGenreStatisticAsync(GenreStaticticQuery query);
        string ExportStatisticAsync(ExportStatisticQuery exportStatisticQuery, IWebHostEnvironment _appEnvironment);
    }
}
