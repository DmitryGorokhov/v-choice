using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly DBContext _context;

        public StatisticRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public IQueryable<Genre> GetGenreStatisticByFilms() 
            => _context.Genre.Include(e => e.Films).OrderBy(e => e.Films.Count);
        public IQueryable<Genre> GetGenreStatisticByRequested()
            => _context.Genre.Include(e => e.Films).OrderBy(e => e.Requested);
    }
}
