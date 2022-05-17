using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PaginationRepository : IPaginationRepository
    {
        private readonly DBContext _context;

        public PaginationRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public async Task<(int, IQueryable<T>)> SplitByPagesAsync<T>(IQueryable<T> collection, int pageNumber, int onPageCount)
        {
            int total = await collection.CountAsync();
            var items = collection.Skip((pageNumber - 1) * onPageCount).Take(onPageCount);

            return (total, items);
        }

        public IQueryable<Film> GetFilmsByGenreId(IQueryable<Film> collection, int genreId)
            => collection.Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null);

        public IQueryable<Film> GetFilmsWithCommentsOnly(IQueryable<Film> collection)
           => collection.Include(e => e.Comments).Where(e => e.Comments.Count != 0);

        public IQueryable<Film> GetFilmsWithRateOnly(IQueryable<Film> collection)
           => collection.Where(e => e.CountRate != 0);

        public IQueryable<Film> GetAllFilms() 
            => _context.Film.Include(e => e.Genres);

        public IQueryable<Film> GetFilmsByCreated(IQueryable<Film> collection)
            => collection.OrderBy(e => e.CreatedAt);

        public IQueryable<Film> GetFilmsByCreatedDesc(IQueryable<Film> collection)
            => collection.OrderByDescending(e => e.CreatedAt);

        public IQueryable<Film> GetFilmsByYear(IQueryable<Film> collection)
            => collection.OrderBy(e => e.Year);

        public IQueryable<Film> GetFilmsByYearDesc(IQueryable<Film> collection)
            => collection.OrderByDescending(e => e.Year);

        public IQueryable<Film> GetFilmsByRate(IQueryable<Film> collection)
            => collection.OrderBy(e => e.AverageRate);

        public IQueryable<Film> GetFilmsByDesc(IQueryable<Film> collection)
            => collection.OrderByDescending(e => e.AverageRate);

        public IQueryable<Film> GetFavoritesByDateDescending(string userId)
            => _context.Favorite.Where(c => c.AuthorId == userId).OrderByDescending(c => c.AddedAt)
                .Select(fav => _context.Film.FirstOrDefault(f => f.Id == fav.FilmId));

        public IQueryable<Film> GetFavoritesByDate(string userId)
            => _context.Favorite.Where(c => c.AuthorId == userId).OrderBy(c => c.AddedAt)
                .Select(fav => _context.Film.FirstOrDefault(f => f.Id == fav.FilmId));

        public async Task<IEnumerable<Comment>> GetCommentsByDateDescendingUserFirst(string userId, int filmId)
        {
            var q1 = await _context.Comment.Where(c => c.FilmId == filmId).Where(c => c.AuthorId == userId).OrderByDescending(c => c.CreatedAt).ToListAsync();
            var q2 = await _context.Comment.Where(c => c.FilmId == filmId).Where(c => c.AuthorId != userId).OrderByDescending(c => c.CreatedAt).ToListAsync();
            return q1.Concat(q2);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByDateUserFirst(string userId, int filmId)
        {
            var q1 = await _context.Comment.Where(c => c.FilmId == filmId).Where(c => c.AuthorId == userId).OrderBy(c => c.CreatedAt).ToListAsync();
            var q2 = await _context.Comment.Where(c => c.FilmId == filmId).Where(c => c.AuthorId != userId).OrderBy(c => c.CreatedAt).ToListAsync();
            return q1.Concat(q2);
        }

        public IQueryable<Comment> GetCommentsByDateDescendingOnly(int filmId)
            => _context.Comment.Where(c => c.FilmId == filmId).OrderByDescending(c => c.CreatedAt);

        public IQueryable<Comment> GetCommentsByDateOnly(int filmId)
            => _context.Comment.Where(c => c.FilmId == filmId).OrderBy(c => c.CreatedAt);

        public IQueryable<Person> GetAllPersons()
            => _context.Person;
    }
}
