using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteFilmAsync(int filmId, string userId);
        Task RemoveFilmFromFavorite(int filmId, string userId);
        Task<bool?> CheckFilmIsAdded(int filmId, string userId);
    }
}
