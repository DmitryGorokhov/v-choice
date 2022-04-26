using BLL.DTO;
using BLL.Query;
using DAL.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IAutorizationService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterQuery reg);
        Task<SignInResult> LogInUserAsync(LoginQuery log);
        Task UserSignOutAsync();
        Task<User> GetCurrentUserModelAsync(ClaimsPrincipal user);
        Task<bool> CheckIfCurrentUserIsAdmin(ClaimsPrincipal user);
        Task<string> GetCurrentUserNameAsync(ClaimsPrincipal user);
    }
}
