using System.Collections.Generic;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Interfaces
{
    public interface IFilmRepository
    {
        IEnumerable<Film> GetAllFilmsAsync();
        Task<Film> GetFilmAsync(int id);
        Task<Film> CreateFilmAsync(Film film);
        Task<int> UpdateFilmAsync(int id, Film film);
        Task<int> DeleteFilmAsync(int id);
    }
}
