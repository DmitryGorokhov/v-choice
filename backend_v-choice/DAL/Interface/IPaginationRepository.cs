using DAL.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IPaginationRepository
    {
        Task<(int, IQueryable<T>)> SplitByPagesAsync<T>(IQueryable<T> collection, int pageNumber, int onPageCount);
        IQueryable<Film> GetFilmsByGenreId(IQueryable<Film> collection, int genreId);
        IQueryable<Film> GetFilmsWithCommentsOnly(IQueryable<Film> collection);
        IQueryable<Film> GetFilmsWithRateOnly(IQueryable<Film> collection);
        IQueryable<Film> GetAllFilms();
        IQueryable<Film> GetFilmsByCreated(IQueryable<Film> collection);
        IQueryable<Film> GetFilmsByCreatedDesc(IQueryable<Film> collection);
        IQueryable<Film> GetFilmsByYear(IQueryable<Film> collection);
        IQueryable<Film> GetFilmsByYearDesc(IQueryable<Film> collection);
        IQueryable<Film> GetFilmsByRate(IQueryable<Film> collection);
        IQueryable<Film> GetFilmsByDesc(IQueryable<Film> collection);
        IQueryable<Film> GetFavoritesByDateDescending(string userId);
        IQueryable<Film> GetFavoritesByDate(string userId);
        Task<IEnumerable<Comment>> GetCommentsByDateDescendingUserFirst(string userId, int filmId);
        Task<IEnumerable<Comment>> GetCommentsByDateUserFirst(string userId, int filmId);
        IQueryable<Comment> GetCommentsByDateDescendingOnly(int filmId);
        IQueryable<Comment> GetCommentsByDateOnly(int filmId);
    }
}
