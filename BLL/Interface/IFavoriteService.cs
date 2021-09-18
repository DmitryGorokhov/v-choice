using BLL.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FilmDTO>> GetAllFavoriteFilmsAsync(ClaimsPrincipal user);
        Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user);
        Task AddFavoriteFilmAsync(FilmDTO film, ClaimsPrincipal user);
        Task RemoveFilmFromFavorite(FilmDTO film, ClaimsPrincipal user);
    }
}
