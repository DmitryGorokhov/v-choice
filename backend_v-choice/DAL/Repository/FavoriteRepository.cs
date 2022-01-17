using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DBContext _context;
        private readonly IUserRepository _users;
        
        public FavoriteRepository(DBContext dbc, IUserRepository ur)
        {
            _context = dbc;
            _users = ur;
        }


        public async Task AddFavoriteFilmAsync(int filmId, ClaimsPrincipal user)
        {
            User author = await _users.GetCurrentUserAsync(user);
            Film film = await _context.Film.FirstOrDefaultAsync(e => e.Id == filmId);

            Favorite fav = new Favorite()
            {
                FilmId = film.Id,
                AddedAt = DateTime.Now,
                AuthorId = author.Id,
                Author = author,
                Film = film
            };

            author.Favorites.Add(fav);
            _context.User.Update(author);
            
            film.InFavorites.Add(fav);
            _context.Film.Update(film);

            _context.Favorite.Add(fav);
            await _context.SaveChangesAsync();
        }

        public async Task<bool?> CheckFilmIsAdded(int filmId, ClaimsPrincipal user)
        {
            string userId = (await _users.GetCurrentUserAsync(user)).Id;
            Favorite item = await _context.Favorite.FirstOrDefaultAsync(e => e.FilmId == filmId && e.AuthorId == userId);
            return item != null;
        }

        public async Task<Pagination<Film>> GetFavoriteFilmsByPageAsync(int pageNumber, int onPageCount, bool commonOrder, ClaimsPrincipal user)
        {
            string userId = (await _users.GetCurrentUserAsync(user)).Id;
            var collection = _context.Favorite.Where(c => c.AuthorId == userId);

            // By date order only.
            collection = commonOrder switch
            {
                // old to new
                false => collection.OrderBy(c => c.AddedAt),
                // new to old, default
                true => collection.OrderByDescending(c => c.AddedAt),
            };

            var favFilms = collection.Select(fav => _context.Film.Find(fav.FilmId));

            var total = await favFilms.CountAsync();
            var items = await favFilms.Skip((pageNumber - 1) * onPageCount).Take(onPageCount).ToListAsync();

            return new Pagination<Film>()
            {
                Items = items,
                TotalCount = total
            };
        }

        public async Task RemoveFilmFromFavorite(int filmId, ClaimsPrincipal user)
        {
            string userId = (await _users.GetCurrentUserAsync(user)).Id;
            Favorite item = await _context.Favorite.FirstOrDefaultAsync(e => e.FilmId == filmId && e.AuthorId == userId);
            _context.Favorite.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
