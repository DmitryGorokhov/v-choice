using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> UserRegisterAsync(RegisterViewModel model);
        Task<SignInResult> UserLogInAsync(LoginViewModel model);
        Task UserSignOutAsync();
        Task<User> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user);
    }
}
