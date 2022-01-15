using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class RateRepository : IRateRepository
    {
        private readonly DBContext _context;
        private readonly IUserRepository _users;

        public RateRepository(DBContext dbc, IUserRepository users)
        {
            _context = dbc;
            _users = users;
        }

        public async Task<Rate> CreateRateAsync(Rate rate, ClaimsPrincipal user)
        {
            User author = await _users.GetCurrentUserAsync(user);
            rate.Author = author;
            rate.AuthorId = author.Id;
            rate.AuthorEmail = author.Email;

            var film = _context.Film.Find(rate.FilmId);
            if (film != null)
            {
                film.RateCollection.Add(rate);
                _context.Film.Update(film);
            }

            author.RateCollection.Add(rate);
            _context.User.Update(author);

            _context.Rate.Add(rate);
            await _context.SaveChangesAsync();

            return rate;
        }

        public async Task DeleteRateAsync(int id)
        {
            try
            {
                Rate item = _context.Rate.Find(id);
                _context.Rate.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int?> GetFilmRate(int filmId, ClaimsPrincipal user)
        {
            User u = await _users.GetCurrentUserAsync(user);
            Rate rate = await _context.Rate.FirstOrDefaultAsync(e => e.FilmId == filmId && e.AuthorId == u.Id);
            if (rate == null) return null;

            return rate.Value;
        }

        public async Task UpdateRateAsync(int id, Rate rate)
        {
            try
            {
                Rate item = _context.Rate.Find(id);
                item.Value = rate.Value;
                _context.Rate.Update(item);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
