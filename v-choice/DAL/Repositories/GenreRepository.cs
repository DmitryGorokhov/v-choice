using System.Collections.Generic;
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
    }
}
