using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Interface
{
    public interface IFilmRepository
    {
        Task<Film> GetFilmAsync(int id);
        Task<Film> CreateFilmAsync(Film film);
        Task UpdateFilmAsync(int id, Film film);
        Task DeleteFilmAsync(int id);
        Task<Pagination<Film>> GetFilmsByPageAsync(int pageNumber, int onPageCount, int genreId);
    }
}
