using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            film.CreatedAt = DateTime.Now;
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

        public async Task<Film> GetFilmAsync(int id)
            => await _context.Film.Include(e => e.Genres).SingleOrDefaultAsync(m => m.Id == id);

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
            Film item = _context.Film.Include(e => e.Genres).First(e => e.Id == id);
            item.Title = film.Title;
            item.Year = film.Year;
            item.Description = film.Description;
            item.Genres = film.Genres;
            item.PosterPath = film.PosterPath;

            _context.Film.Update(item);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Film> FilterByGenreId(IQueryable<Film> collection, int gId)
        {
            return collection
                .Include(e => e.Genres)
                .Where(e => e.Genres.FirstOrDefault(g => g.Id == gId) != null)
                .Select(e => e);
        }

        private IQueryable<Film> FilterWithCommentsOnly(IQueryable<Film> collection)
        {
            return collection.Include(e => e.Comments).Where(e => e.Comments.Count != 0);
        }

        private IQueryable<Film> FilterWithRateOnly(IQueryable<Film> collection)
        {
            return collection.Where(e => e.CountRate != 0);
        }

        private IQueryable<Film> AcceptFilters(int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            IQueryable<Film> collection = _context.Film;

            if (gId > 0)
            {
                collection = FilterByGenreId(collection, gId);
            }

            if (hasCommentsOnly)
            {
                collection = FilterWithCommentsOnly(collection);
            }

            if (withRateOnly)
            {
                collection = FilterWithRateOnly(collection);
            }

            return collection;
        }

        public async Task<Pagination<Film>> GetFilmsSortedByCreatedAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly)
                .OrderBy(e => e.CreatedAt);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Film>> GetFilmsSortedByCreatedDescAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly)
                .OrderByDescending(e => e.CreatedAt);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Film>> GetFilmsSortedByYearAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly)
                .OrderBy(e => e.Year);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Film>> GetFilmsSortedByYearDescAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly)
                .OrderByDescending(e => e.Year);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Film>> GetFilmsSortedByRateAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly)
                .OrderBy(e => e.AverageRate);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Film>> GetFilmsSortedByRateDescAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly)
                .OrderByDescending(e => e.AverageRate);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Film>> GetFilmsAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilters(gId, hasCommentsOnly, withRateOnly);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Film> SetPosterPathAsync(int id, string posterPath)
        {
            Film film = _context.Film.Find(id);
            film.PosterPath = posterPath;
            await _context.SaveChangesAsync();
            
            return film;
        }
    }
}
