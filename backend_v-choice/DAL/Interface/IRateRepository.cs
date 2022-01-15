using DAL.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IRateRepository
    {
        Task<Rate> CreateRateAsync(Rate rate, ClaimsPrincipal user);
        Task UpdateRateAsync(int id, Rate rate);
        Task DeleteRateAsync(int id);
        Task<int?> GetFilmRate(int filmId, ClaimsPrincipal user);
    }
}
