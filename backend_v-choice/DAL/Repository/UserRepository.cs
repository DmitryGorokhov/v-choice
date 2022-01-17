using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL.Interface;
using DAL.Model;

namespace DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(DBContext dbc, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = dbc;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal user) => await _userManager.GetUserAsync(user);

        public async Task<SignInResult> UserLogInAsync(LoginModel model)
            => await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

        public async Task<IdentityResult> UserRegisterAsync(RegisterModel model)
        {
            User user = new User { Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // добавление роли пользователя
                await _userManager.AddToRoleAsync(user, "user");

                // установка куки
                await _signInManager.SignInAsync(user, false);
            }

            return result;
        }

        public async Task UserSignOutAsync() => await _signInManager.SignOutAsync();
    }
}
