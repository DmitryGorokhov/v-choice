using BLL.DTO;
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
        Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}
