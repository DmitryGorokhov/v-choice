using DAL.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteFilmAsync(int filmId, ClaimsPrincipal user);
        Task RemoveFilmFromFavorite(int filmId, ClaimsPrincipal user);
        Task<bool?> CheckFilmIsAdded(int filmId, ClaimsPrincipal user);
        Task<Pagination<Film>> GetFavoriteFilmsByPageAsync(int pageNumber, int onPageCount, bool commonOrder, ClaimsPrincipal user);
    }
}
