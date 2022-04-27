using DAL.Model;
using System.Linq;

namespace DAL.Interface
{
    public interface IStatisticRepository
    {
        IQueryable<Genre> GetGenreStatisticByFilms();
        IQueryable<Genre> GetGenreStatisticByRequested();
    }
}
