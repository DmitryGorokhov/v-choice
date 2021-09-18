using BLL.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IAutorizationService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDTO reg);
        Task<SignInResult> LogInUserAsync(LoginDTO log);
        Task UserSignOutAsync();
        Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}
