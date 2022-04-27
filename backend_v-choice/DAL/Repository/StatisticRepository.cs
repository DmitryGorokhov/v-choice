using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly DBContext _context;

        public StatisticRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public GeneralStatistic GetGeneralStatistic()
        {
            GeneralStatistic obj = new GeneralStatistic();

            obj.FilmsTotal = _context.Film.Count();
            obj.FilmsRated = _context.Film.Count(e => e.CountRate != 0);
            obj.FilmsNotRated = obj.FilmsTotal - obj.FilmsRated;
            obj.FilmsCommented = _context.Film.Include(e => e.Comments).Count(e => e.Comments.Count != 0);
            obj.FilmsNotCommented = obj.FilmsTotal - obj.FilmsCommented;

            var sortedByYear = _context.Film.OrderBy(e => e.Year);

            obj.MinYear = sortedByYear.First().Year;
            obj.MaxYear = sortedByYear.Last().Year;

            obj.CommentsTotal = _context.Comment.Count();
            obj.CommentsMax = _context.Film.Include(e => e.Comments).OrderBy(e => e.Comments.Count).Last().Comments.Count;

            return obj;
        }

        public IQueryable<Genre> GetGenreStatisticByFilms()
            => _context.Genre.Include(e => e.Films).OrderByDescending(e => e.Films.Count);

        public IQueryable<Genre> GetGenreStatisticByRequested()
            => _context.Genre.Include(e => e.Films).OrderByDescending(e => e.Requested);

        IQueryable<Film> IStatisticRepository.GetFilmStatisticByComments()
            => _context.Film.Include(e => e.InFavorites).Include(e => e.Comments).OrderByDescending(e => e.Comments.Count);

        IQueryable<Film> IStatisticRepository.GetFilmStatisticByCountRate()
            => _context.Film.Include(e => e.InFavorites).Include(e => e.Comments).OrderByDescending(e => e.CountRate);

        IQueryable<Film> IStatisticRepository.GetFilmStatisticByFavorites()
            => _context.Film.Include(e => e.InFavorites).Include(e => e.Comments).OrderByDescending(e => e.InFavorites.Count);

        IQueryable<Film> IStatisticRepository.GetFilmStatisticByRate()
            => _context.Film.Include(e => e.InFavorites).Include(e => e.Comments).OrderByDescending(e => e.AverageRate);

        IQueryable<Film> IStatisticRepository.GetFilmStatisticByRequested()
            => _context.Film.Include(e => e.InFavorites).Include(e => e.Comments).OrderByDescending(e => e.Requested);
    }
}
