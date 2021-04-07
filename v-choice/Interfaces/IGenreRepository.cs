using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAllGenres();
    }
}
