using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace v_choice.Models
{
    public class Genre
    {
        public Genre() : base()
        {
            Films = new HashSet<Film>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Film> Films { get; set; }
    }
}
