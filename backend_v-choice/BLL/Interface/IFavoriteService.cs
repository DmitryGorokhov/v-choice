using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IFavoriteService
    {
        Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user);
        Task AddFavoriteFilmAsync(int filmId, ClaimsPrincipal user);
        Task RemoveFilmFromFavorite(int filmId, ClaimsPrincipal user);
    }
}
