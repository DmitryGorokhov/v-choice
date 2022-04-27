using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Pagination<Comment>> GetCommentsByDateDescendingOnlyAsync(int pageNumber, int onPageCount, int filmId)
        {
            var collection = _context.Comment.Where(c => c.FilmId == filmId).OrderByDescending(c => c.CreatedAt);
            
            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Comment>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Comment>> GetCommentsByDateDescendingUserFirstAsync(int pageNumber, int onPageCount, int filmId, string userId)
        {
            var collection = _context.Comment.Where(c => c.FilmId == filmId);
            collection = collection.Where(c => c.AuthorId == userId).OrderByDescending(c => c.CreatedAt)
                .Union(collection.Where(c => c.AuthorId != userId).OrderByDescending(c => c.CreatedAt));

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Comment>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Comment>> GetCommentsByDateOnlyAsync(int pageNumber, int onPageCount, int filmId)
        {
            var collection = _context.Comment.Where(c => c.FilmId == filmId).OrderBy(c => c.CreatedAt);
            
            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Comment>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Comment>> GetCommentsByDateUserFirstAsync(int pageNumber, int onPageCount, int filmId, string userId)
        {
            var collection = _context.Comment.Where(c => c.FilmId == filmId);
            collection = collection.Where(c => c.AuthorId == userId).OrderBy(c => c.CreatedAt)
                .Union(collection.Where(c => c.AuthorId != userId).OrderBy(c => c.CreatedAt));

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Comment>() { TotalCount = total, Items = items };
        }

        public async Task<(int, IQueryable<T>)> SplitByPagesAsync<T>(IQueryable<T> collection, int pageNumber, int onPageCount)
        {
            int total = await collection.CountAsync();
            var items = collection.Skip((pageNumber - 1) * onPageCount).Take(onPageCount);

            return (total, items);
        }

        public async Task<Pagination<Film>> GetFavoritesByDateAsync(int pageNumber, int onPageCount, string userId)
        {
            var collection = _context.Favorite.Where(c => c.AuthorId == userId).OrderBy(c => c.AddedAt)
                .Select(fav => _context.Film.FirstOrDefault(f => f.Id == fav.FilmId));

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFavoritesByDateDescendingAsync(int pageNumber, int onPageCount, string userId)
        {
            var collection = _context.Favorite.Where(c => c.AuthorId == userId).OrderByDescending(c => c.AddedAt)
                .Select(fav => _context.Film.FirstOrDefault(f => f.Id == fav.FilmId));

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        private IQueryable<Film> FilterByGenreId(IQueryable<Film> collection, int gId)
        {
            return collection.Where(e => e.Genres.FirstOrDefault(g => g.Id == gId) != null);
        }

        private IQueryable<Film> FilterWithCommentsOnly(IQueryable<Film> collection)
        {
            return collection.Include(e => e.Comments).Where(e => e.Comments.Count != 0);
        }

        private IQueryable<Film> FilterWithRateOnly(IQueryable<Film> collection)
        {
            return collection.Where(e => e.CountRate != 0);
        }

        private IQueryable<Film> AcceptFilmFilters(int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            IQueryable<Film> collection = _context.Film.Include(e => e.Genres);

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
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly).OrderBy(e => e.CreatedAt);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFilmsSortedByCreatedDescAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly).OrderByDescending(e => e.CreatedAt);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFilmsSortedByYearAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly).OrderBy(e => e.Year);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFilmsSortedByYearDescAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly).OrderByDescending(e => e.Year);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFilmsSortedByRateAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly).OrderBy(e => e.AverageRate);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFilmsSortedByRateDescAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly).OrderByDescending(e => e.AverageRate);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }

        public async Task<Pagination<Film>> GetFilmsAsync(int pageNumber, int onPageCount, int gId, bool hasCommentsOnly, bool withRateOnly)
        {
            var collection = AcceptFilmFilters(gId, hasCommentsOnly, withRateOnly);

            (int total, var items) = await SplitByPagesAsync(collection, pageNumber, onPageCount);

            return new Pagination<Film>() { TotalCount = total, Items = items };
        }
    }
}
