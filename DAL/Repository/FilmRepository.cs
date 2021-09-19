using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interface;
using DAL.Model;

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
            {
                foreach (var genre in film.Genres)
                {
                    var g = _context.Genre.FirstOrDefault(e => e.Id == genre.Id);
                    if (g != null)
                    {
                        g.Films.Add(film);
                        _context.Genre.Update(g);
                    }
                }
            }
            film.Genres = new HashSet<Genre>();
            _context.Film.Add(film);
            await _context.SaveChangesAsync();
            return film;
        }

        public async Task DeleteFilmAsync(int id)
        {
            try
            {
                var item = _context.Film.Find(id);
                _context.Film.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Film> GetAllFilms()
        {
            return _context.Film.Include(f => f.Genres);
        }

        public async Task<Film> GetFilmAsync(int id)
        {
            return await _context.Film.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Pagination<Film>> GetFilmsByPageAsync(int pageNumber, int onPageCount, int genreId)
        {
            var itemsQuery = genreId >= 0
                ? _context.Film.Include(g => g.Genres).Where(e => e.Genres.FirstOrDefault(g => g.Id == genreId) != null)
                : _context.Film;

            int total = await itemsQuery.CountAsync();
            var items = await itemsQuery.Skip((pageNumber - 1) * onPageCount).Take(onPageCount).ToListAsync();

            return new Pagination<Film>()
            {
                Items = items,
                TotalCount = total
            };
        }

        public async Task UpdateFilmAsync(int id, Film film)
        {
            try
            {
                var item = _context.Film.Find(id);
                item.Title = film.Title;
                item.Year = film.Year;
                item.Description = film.Description;
                _context.Film.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
