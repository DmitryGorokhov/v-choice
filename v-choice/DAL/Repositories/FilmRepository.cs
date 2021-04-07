using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Interfaces;
using v_choice.Models;

namespace v_choice.DAL.Repositories
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

        public async Task<int> DeleteFilmAsync(int id)
        {
            var item = _context.Film.Find(id);
            _context.Film.Remove(item);
            await _context.SaveChangesAsync();
            return 0;
        }

        public IEnumerable<Film> GetAllFilmsAsync()
        {
            return _context.Film.Include(f => f.Genres);
        }

        public async Task<Film> GetFilmAsync(int id)
        {
            return await _context.Film.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> UpdateFilmAsync(int id, Film film)
        {
            var item = _context.Film.Find(id);
            item.Title = film.Title;
            item.Year = film.Year;
            item.Description = film.Description;
            _context.Film.Update(item);
            await _context.SaveChangesAsync();
            return 0;
        }
    }
}
