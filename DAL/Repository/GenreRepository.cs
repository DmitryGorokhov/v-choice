using System.Collections.Generic;
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

        public IEnumerable<Genre> GetAllGenres() => _context.Genre;
    }
}
