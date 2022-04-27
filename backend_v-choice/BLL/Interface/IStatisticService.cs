using BLL.DTO;
using BLL.Query;
using DAL.Model;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IStatisticService
    {
        GeneralStatistic GetGeneralStatistic();
        Task<PaginationDTO<FilmStatisticDTO>> GetFilmStatisticAsync(FilmStaticticQuery query);
        Task<PaginationDTO<GenreStatisticDTO>> GetGenreStatisticAsync(GenreStaticticQuery query);
    }
}
