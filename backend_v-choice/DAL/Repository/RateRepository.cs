using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class RateRepository : IRateRepository
    {
        private readonly DBContext _context;

        public RateRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public async Task<Rate> CreateRateAsync(Rate rate, string userId)
        {
            User author = await _context.User.FirstOrDefaultAsync(e => e.Id == userId);
            rate.Author = author;
            rate.AuthorId = author.Id;
            rate.AuthorEmail = author.Email;

            var film = _context.Film.Find(rate.FilmId);
            if (film != null)
            {
                film.CountRate++;
                film.TotalRate += rate.Value;
                film.AverageRate = (float)film.TotalRate / film.CountRate;
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
            Rate item = _context.Rate.Find(id);

            var film = _context.Film.Find(item.FilmId);
            if (film != null)
            {
                film.CountRate--;
                if (film.CountRate == 0)
                {
                    film.TotalRate = 0;
                    film.AverageRate = 0;
                }
                else
                {
                    film.TotalRate -= item.Value;
                    film.AverageRate = (float)film.TotalRate / film.CountRate;
                }

                _context.Film.Update(film);
            }

            _context.Rate.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Rate> GetFilmRate(int filmId, string userId)
        {
            // User u = await _context.User.FirstOrDefaultAsync(e => e.Id == userId);
            Rate rate = await _context.Rate.FirstOrDefaultAsync(e => e.FilmId == filmId && e.AuthorId == userId);

            return rate;
        }

        public async Task UpdateRateAsync(int id, Rate rate)
        {
            Rate item = _context.Rate.Find(id);

            var film = _context.Film.Find(rate.FilmId);
            if (film != null)
            {
                film.TotalRate += rate.Value - item.Value;
                film.AverageRate = (float)film.TotalRate / film.CountRate;
                _context.Film.Update(film);
            }

            item.Value = rate.Value;
            _context.Rate.Update(item);

            await _context.SaveChangesAsync();
        }
    }
}
