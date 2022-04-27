using DAL.Model;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IFilmRepository
    {
        Task<Film> GetFilmAsync(int id);
        Task<Film> CreateFilmAsync(Film film);
        Task UpdateFilmAsync(int id, Film film);
        Task DeleteFilmAsync(int id);
        Task<Film> SetPosterPathAsync(int id, string posterPath);
        Task FilmRequestedCounter(int id);
    }
}
