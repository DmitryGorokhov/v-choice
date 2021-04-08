using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using v_choice.Interfaces;
using v_choice.Models;

namespace v_choice.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<SignInResult> UserLogInAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        }

        public async Task<IdentityResult> UserRegisterAsync(RegisterViewModel model)
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

        public async Task UserSignOutAsync()
        {
            // Удаление куки
            await _signInManager.SignOutAsync();
        }
    }
}
