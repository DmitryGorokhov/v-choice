using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> UserRegisterAsync(RegisterViewModel model);
        Task<SignInResult> UserLogInAsync(LoginViewModel model);
        Task UserSignOutAsync();
        Task<User> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<IEnumerable<Film>> GetAllFavoriteFilmsAsync(ClaimsPrincipal user);
        Task AddFavoriteFilmAsync(Film film, ClaimsPrincipal user);
        Task RemoveFilmFromFavorite(Film film, ClaimsPrincipal user);
        Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user);
    }
}
