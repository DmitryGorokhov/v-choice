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

            if (film.Persons.Count != 0)
                foreach (var item in film.Persons)
                {
                    Person p = _context.Person.FirstOrDefault(e => e.Id == item.PersonId);
                    if (p != null)
                    {
                        p.Participations.Add(item);
                        _context.Person.Update(p);
                    }
                }

            film.Genres = new HashSet<Genre>();
            film.CreatedAt = DateTime.Now;
            film.Requested = 0;
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
            => await _context.Film.Include(e => e.Genres).Include(e => e.Studio).Include(e => e.Persons).ThenInclude(e => e.Person)
                        .SingleOrDefaultAsync(m => m.Id == id);

        public async Task UpdateFilmAsync(int id, Film film)
        {
            Film item = _context.Film.Include(e => e.Genres).Include(e => e.Persons).First(e => e.Id == id);
            item.Title = film.Title;
            item.Year = film.Year;
            item.Description = film.Description;

            foreach (var g in item.Genres)
            {
                if (film.Genres.FirstOrDefault(e => e.Id == g.Id) == null)
                {
                    item.Genres.Remove(g);
                }
            }
            foreach (var g in film.Genres)
            {
                if (item.Genres.FirstOrDefault(e => e.Id == g.Id) == null)
                {
                    item.Genres.Add(g);
                }
            }

            foreach (var p in item.Persons)
            {
                if (film.Persons.FirstOrDefault(e => e.PersonId == p.PersonId && e.Role == p.Role) == null)
                {
                    item.Persons.Remove(p);
                }
            }
            foreach (var p in film.Persons)
            {
                if (item.Persons.FirstOrDefault(e => e.PersonId == p.PersonId && e.Role == p.Role) == null)
                {
                    item.Persons.Add(p);
                }
            }

            item.PosterPath = film.PosterPath;
            item.VideoToken = film.VideoToken;

            _context.Film.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Film> SetPosterPathAsync(int id, string posterPath)
        {
            Film film = _context.Film.Find(id);
            film.PosterPath = posterPath;
            await _context.SaveChangesAsync();

            return film;
        }

        public async Task FilmRequestedCounter(int id)
        {
            Film film = _context.Film.Find(id);
            film.Requested++;
            await _context.SaveChangesAsync();
        }
    }
}
