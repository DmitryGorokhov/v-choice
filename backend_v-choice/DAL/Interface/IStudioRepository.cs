using DAL.Model;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStudioRepository
    {
        IQueryable<Studio> GetAllStudios();
        Task<Studio> CreateStudioAsync(Studio s);
        Task UpdateStudioAsync(int id, Studio s);
        Task DeleteStudioAsync(int id);
    }
}
