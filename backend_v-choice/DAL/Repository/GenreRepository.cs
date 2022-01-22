using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interface;
using DAL.Model;

namespace DAL.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DBContext _context;

        public GenreRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateGenreAsync(Genre g)
        {
            _context.Genre.Add(g);
            await _context.SaveChangesAsync();

            return g;
        }

        public async Task DeleteGenreAsync(int id)
        {
            Genre item = _context.Genre.Find(id);
            _context.Genre.Remove(item);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Genre> GetAllGenres() => _context.Genre;

        public async Task UpdateGenreAsync(int id, Genre g)
        {
            Genre item = _context.Genre.Find(id);
            item.Value = g.Value;
            _context.Genre.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
