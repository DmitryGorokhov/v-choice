using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interface
{
    public interface IFavoriteService
    {
        Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user);
        Task AddFavoriteFilmAsync(FilmDTO film, ClaimsPrincipal user);
        Task RemoveFilmFromFavorite(FilmDTO film, ClaimsPrincipal user);
    }
}
