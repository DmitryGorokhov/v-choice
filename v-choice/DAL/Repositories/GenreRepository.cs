using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Interfaces;
using v_choice.Models;

namespace v_choice.DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DBContext _context;
        public GenreRepository(DBContext context)
        {
            _context = context;
        }
        public IEnumerable<Genre> GetAllGenres()
        {
            return _context.Genre;
        }

        public async Task<ICollection<Film>> GetFilmsByGenreIdAsync(int id)
        {
            Genre g = await _context.Genre.Include(g => g.Films).FirstOrDefaultAsync(e=>e.Id==id);
            if (g == null)
                return null;
            return g.Films;
        }
    }
}
