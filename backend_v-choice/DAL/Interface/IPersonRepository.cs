using DAL.Model;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IPersonRepository
    {
        Task<Person> CreatePersonAsync(Person p);
        Task<Person> SetPhotoPathAsync(int id, string photoPath);
        Task DeletePersonAsync(int id);
        Task UpdatePersonAsync(int id, Person p);
        IQueryable<Person> GetAllPersons();
    }
}
