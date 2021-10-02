using System.Collections.Generic;
using DAL.Model;

namespace DAL.Interface
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAllGenres();
    }
}
