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
        Task<(LoginDTO,IdentityResult)> RegisterUserAsync(RegisterQuery reg);
        Task<LoginDTO> LogInUserAsync(LoginQuery log);
        Task UserSignOutAsync();
        Task<User> GetCurrentUserModelAsync(ClaimsPrincipal user);
        Task<AuthenticatedUserDTO> GetAuthenticatedUserAsync(ClaimsPrincipal user);
    }
}
