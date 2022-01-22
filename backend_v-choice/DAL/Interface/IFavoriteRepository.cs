using DAL.Model;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteFilmAsync(int filmId, string userId);
        Task RemoveFilmFromFavorite(int filmId, string userId);
        Task<bool?> CheckFilmIsAdded(int filmId, string userId);
        Task<Pagination<Film>> GetByDateAsync(int pageNumber, int onPageCount, string userId);
        Task<Pagination<Film>> GetByDateDescendingAsync(int pageNumber, int onPageCount, string userId);
    }
}
