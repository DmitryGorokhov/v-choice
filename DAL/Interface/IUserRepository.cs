using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Interface
{
    public interface IUserRepository
    {
        Task<IdentityResult> UserRegisterAsync(RegisterModel model);
        Task<SignInResult> UserLogInAsync(LoginModel model);
        Task UserSignOutAsync();
        Task<User> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<IEnumerable<Film>> GetAllFavoriteFilmsAsync(ClaimsPrincipal user);
        Task AddFavoriteFilmAsync(Film film, ClaimsPrincipal user);
        Task RemoveFilmFromFavorite(Film film, ClaimsPrincipal user);
        Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user);
        Task<Pagination<Film>> GetFavoriteFilmsByPageAsync(int pageNumber, int onPageCount);
    }
}
