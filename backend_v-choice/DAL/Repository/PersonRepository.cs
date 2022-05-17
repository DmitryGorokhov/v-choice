using DAL.Interface;
using DAL.Model;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DBContext _context;

        public PersonRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public async Task<Person> CreatePersonAsync(Person p)
        {
            _context.Person.Add(p);
            await _context.SaveChangesAsync();

            return p;
        }

        public async Task DeletePersonAsync(int id)
        {
            Person person = _context.Person.Find(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
        }

        public async Task<Person> SetPhotoPathAsync(int id, string photoPath)
        {
            Person item = _context.Person.Find(id);
            item.PhotoPath = photoPath;
            _context.Person.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task UpdatePersonAsync(int id, Person p)
        {
            Person item = _context.Person.Find(id);
            item.FullName = p.FullName;
            _context.Person.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
