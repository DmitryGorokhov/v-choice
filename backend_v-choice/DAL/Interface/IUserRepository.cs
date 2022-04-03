using DAL.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUserRepository
    {
        Task<IdentityResult> UserRegisterAsync(RegisterModel model);
        Task<SignInResult> UserLogInAsync(LoginModel model);
        Task UserSignOutAsync();
        Task<User> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<bool> CheckUserHasAdminRoleAsync(User user);
    }
}
