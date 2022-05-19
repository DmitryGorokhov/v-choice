using DAL.Model;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStatisticRepository
    {
        IQueryable<Genre> GetGenreStatisticByFilms();
        IQueryable<Genre> GetGenreStatisticByRequested();
        IQueryable<Film> GetFilmStatisticByRequested();
        IQueryable<Film> GetFilmStatisticByFavorites();
        IQueryable<Film> GetFilmStatisticByComments();
        IQueryable<Film> GetFilmStatisticByCountRate();
        IQueryable<Film> GetFilmStatisticByRate();
        GeneralStatistic GetGeneralStatistic();
        object GetDataForCatalogFilters();
    }
}
