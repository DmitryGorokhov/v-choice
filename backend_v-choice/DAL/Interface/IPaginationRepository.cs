using DAL.Model;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IPaginationRepository
    {
        Task<Pagination<Comment>> GetCommentsByDateOnlyAsync(int pageNumber, int onPageCount, int filmId);
        Task<Pagination<Comment>> GetCommentsByDateDescendingOnlyAsync(int pageNumber, int onPageCount, int filmId);
        Task<Pagination<Comment>> GetCommentsByDateUserFirstAsync(int pageNumber, int onPageCount, int filmId, string userId);
        Task<Pagination<Comment>> GetCommentsByDateDescendingUserFirstAsync(int pageNumber, int onPageCount, int filmId, string userId);
        Task<Pagination<Film>> GetFavoritesByDateAsync(int pageNumber, int onPageCount, string userId);
        Task<Pagination<Film>> GetFavoritesByDateDescendingAsync(int pageNumber, int onPageCount, string userId);
        Task<Pagination<Film>> GetFilmsSortedByCreatedAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByCreatedDescAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByYearAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByYearDescAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByRateAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsSortedByRateDescAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<Pagination<Film>> GetFilmsAsync(int pageNumber, int onPageCount, int v1, bool v2, bool v3);
        Task<(int, IQueryable<T>)> SplitByPagesAsync<T>(IQueryable<T> collection, int pageNumber, int onPageCount);
    }
}
