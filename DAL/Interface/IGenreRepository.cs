using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Interface
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAllGenres();
        Task<ICollection<Film>> GetFilmsByGenreIdAsync(int id);
    }
}
