using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Interface;
using DAL.Model;
using System.Security.Claims;

namespace DAL.Repository
{
    public class FilmRepository : IFilmRepository
    {
        private readonly DBContext _context;

        public FilmRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public async Task<Film> CreateFilmAsync(Film film)
        {
            if (film.Genres.Count != 0)
                foreach (var genre in film.Genres)
                {
                    Genre g = _context.Genre.FirstOrDefault(e => e.Id == genre.Id);
                    if (g != null)
                    {
                        g.Films.Add(film);
                        _context.Genre.Update(g);
                    }
                }

            film.Genres = new HashSet<Genre>();
            _context.Film.Add(film);
            await _context.SaveChangesAsync();

            return film;
        }

        public async Task DeleteFilmAsync(int id)
        {
            Film item = _context.Film.Find(id);
            _context.Film.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Film> GetFilmAsync(int id) => await _context.Film.SingleOrDefaultAsync(m => m.Id == id);
        
        //public async Task<Pagination<Film>> GetAsync(int pageNumber, int onPageCount)
        //{
        //    var collection = _context.Film;
        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}
        
        //public async Task<Pagination<Film>> GetByGenreAsync(int pageNumber, int onPageCount, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsAsync(int pageNumber, int onPageCount)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsByGenreAsync(int pageNumber, int onPageCount, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetNoUserRateAsync(int pageNumber, int onPageCount, string userId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetNoUserRateByGenreAsync(int pageNumber, int onPageCount, string userId, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsNoUserRateAsync(int pageNumber, int onPageCount, string userId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0)
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsNoUserRateByGenreAsync(int pageNumber, int onPageCount, string userId, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0)
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetAsync(int pageNumber, int onPageCount)
        //{
        //    var collection = _context.Film;
        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetByGenreAsync(int pageNumber, int onPageCount, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsAsync(int pageNumber, int onPageCount)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsByGenreAsync(int pageNumber, int onPageCount, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetNoUserRateAsync(int pageNumber, int onPageCount, string userId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetNoUserRateByGenreAsync(int pageNumber, int onPageCount, string userId, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsNoUserRateAsync(int pageNumber, int onPageCount, string userId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0)
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        //public async Task<Pagination<Film>> GetHasCommentsNoUserRateByGenreAsync(int pageNumber, int onPageCount, string userId, int genreId)
        //{
        //    var collection = _context.Film
        //            .Include(e => e.Genres)
        //            .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
        //            .Include(e => e.Comments)
        //            .Where(e => e.Comments.Count != 0)
        //            .Include(e => e.RateCollection)
        //            .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);

        //    return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        //}

        public async Task<Pagination<Film>> GetFilmsByPageAsync(
            int pageNumber,
            int onPageCount,
            int genreId,
            int sortType,
            bool commonOrder,
            bool hasCommentsOnly,
            bool withoutUserRateOnly,
            string userId)
        {
            var collection = genreId > 0
                ? _context.Film
                    .Include(e => e.Genres)
                    .Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null).Select(e => e)
                : _context.Film;

            if (hasCommentsOnly)
            {
                collection = collection
                    .Include(e => e.Comments)
                    .Where(e => e.Comments.Count != 0);
            }

            if (withoutUserRateOnly)
            {
                collection = collection
                    .Include(e => e.RateCollection)
                    .Where(e => e.RateCollection.FirstOrDefault(r => r.AuthorId == userId) == null);
            }

            collection = sortType switch
            {
                // by created date
                1 when commonOrder => collection.OrderByDescending(e => e.CreatedAt),
                1 when !commonOrder => collection.OrderBy(e => e.CreatedAt),
                // by year
                2 when commonOrder => collection.OrderByDescending(e => e.Year),
                2 when !commonOrder => collection.OrderBy(e => e.Year),
                // by rate
                3 when commonOrder => collection.OrderByDescending(e => e.AverageRate),
                3 when !commonOrder => collection.OrderBy(e => e.AverageRate),
                // none
                _ => collection
            };

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        private async Task<Pagination<Film>> SplitByPagesAsync(IQueryable<Film> collection, int pageNumber, int onPageCount)
        {
            var total = await collection.CountAsync();
            var items = await collection
                .Include(e => e.Genres)
                .Skip((pageNumber - 1) * onPageCount)
                .Take(onPageCount)
                .ToListAsync();

            return new Pagination<Film>()
            {
                Items = items,
                TotalCount = total
            };
        }

        public async Task UpdateFilmAsync(int id, Film film)
        {
            Film item = _context.Film.Find(id);
            item.Title = film.Title;
            item.Year = film.Year;
            item.Description = film.Description;
            _context.Film.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
