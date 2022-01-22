using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Interface
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAllGenres();
        Task<Genre> CreateGenreAsync(Genre g);
        Task DeleteGenreAsync(int id);
        Task UpdateGenreAsync(int id, Genre g);
    }
}
