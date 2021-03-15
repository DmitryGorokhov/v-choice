using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace v_choice.Models
{
    public partial class Viewer : User
    {
        public Viewer() : base()
        {
            Favorites = new HashSet<Film>();
        }

        public virtual ICollection<Film> Favorites { get; set; }
    }
}
