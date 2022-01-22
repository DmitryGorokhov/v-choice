using DAL.Model;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IRateRepository
    {
        Task<Rate> CreateRateAsync(Rate rate, string userId);
        Task UpdateRateAsync(int id, Rate rate);
        Task DeleteRateAsync(int id);
        Task<Rate> GetFilmRate(int filmId, string userId);
    }
}
