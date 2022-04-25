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
        Task<Pagination<Film>> GetFilmsSortedByCreatedAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByCreatedDescAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByYearAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByYearDescAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByRateAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByRateDescAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Film> SetPosterPathAsync(int id, string posterPath);
    }
}
