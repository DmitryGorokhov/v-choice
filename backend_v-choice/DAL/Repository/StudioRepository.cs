using DAL.Interface;
using DAL.Model;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class StudioRepository: IStudioRepository
    {
        private readonly DBContext _context;

        public StudioRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public async Task<Studio> CreateStudioAsync(Studio s)
        {
            _context.Studio.Add(s);
            await _context.SaveChangesAsync();

            return s;
        }

        public async Task DeleteStudioAsync(int id)
        {
            Studio item = _context.Studio.Find(id);
            _context.Studio.Remove(item);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Studio> GetAllStudios()
            => _context.Studio;

        public async Task UpdateStudioAsync(int id, Studio s)
        {
            Studio item = _context.Studio.Find(id);
            item.Name = s.Name;
            _context.Studio.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
