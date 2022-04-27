using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DBContext _context;
        
        public FavoriteRepository(DBContext dbc)
        {
            _context = dbc;
        }


        public async Task AddFavoriteFilmAsync(int filmId, string userId)
        {
            User author = await _context.User.FirstOrDefaultAsync(e => e.Id == userId);
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

        public async Task<bool?> CheckFilmIsAdded(int filmId, string userId)
        {
            Favorite item = await _context.Favorite
                .FirstOrDefaultAsync(e => e.FilmId == filmId && e.AuthorId == userId);

            return item != null;
        }
                
        public async Task RemoveFilmFromFavorite(int filmId, string userId)
        {
            Favorite item = await _context.Favorite
                .FirstOrDefaultAsync(e => e.FilmId == filmId && e.AuthorId == userId);
            
            _context.Favorite.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
