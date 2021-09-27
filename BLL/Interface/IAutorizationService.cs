using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BLL.DTO;

namespace BLL.Interface
{
    public interface IAutorizationService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterQuery reg);
        Task<SignInResult> LogInUserAsync(LoginQuery log);
        Task UserSignOutAsync();
        Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<string> GetCurrentUserEmailAsync(ClaimsPrincipal user);
    }
}
