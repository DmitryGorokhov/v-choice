using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interface;
using DAL.Model;
using System.Security.Claims;
using System.Linq;
using System;

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

        public async Task AddFavoriteFilmAsync(Film film, ClaimsPrincipal user)
        {
            User u = await GetCurrentUserAsync(user);
            Film f = await _context.Film.Include(c => c.Users).FirstOrDefaultAsync(e => e.Id == film.Id);
            if (f == null)
                return;

            f.Users.Add(u);
            _context.Film.Update(f);
            await _context.SaveChangesAsync();
        }

        public async Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user)
        {
            User u = await GetCurrentUserAsync(user);
            Film f = await _context.Film.Include(c => c.Users).FirstOrDefaultAsync(e => e.Id == id);
            if (f == null)
                return null;

            return f.Users.Contains(u);
        }

        public async Task<IEnumerable<Film>> GetAllFavoriteFilmsAsync(ClaimsPrincipal user)
        {
            User u = await GetCurrentUserAsync(user);
            return _context.Film.Where(e => e.Users.Contains(u));
        }

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<Pagination<Film>> GetFavoriteFilmsByPageAsync(int pageNumber, int onPageCount)
        {
            int total = await _context.Film.CountAsync();
            var items = await _context.Film.Skip((pageNumber - 1) * onPageCount).Take(onPageCount).ToListAsync();
            
            return new Pagination<Film>()
            { 
                Items = items,
                TotalCount = total
            };
        }

        public async Task RemoveFilmFromFavorite(Film film, ClaimsPrincipal user)
        {
            try
            {
                User u = await GetCurrentUserAsync(user);
                Film f = await _context.Film.Include(c=>c.Users).FirstOrDefaultAsync(e => e.Id == film.Id);
                if (f == null)
                    return;
                f.Users.Remove(u);
                _context.Film.Update(f);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<SignInResult> UserLogInAsync(LoginModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        }

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

        public async Task UserSignOutAsync()
        {
            // Удаление куки
            await _signInManager.SignOutAsync();
        }
    }
}
