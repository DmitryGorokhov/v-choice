using BLL.DTO;
using BLL.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IStatisticService
    {
        Task<GeneralStatisticDTO> GetGeneralStatisticAsync();
        Task<ICollection<FilmStatisticDTO>> GetFilmStatisticAsync(FilmStaticticQuery query);
        Task<ICollection<GenreStatisticDTO>> GetGenreStatisticAsync(GenreStaticticQuery query);
    }
}
