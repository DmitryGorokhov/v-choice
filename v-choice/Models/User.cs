using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace v_choice.Models
{
    public partial class User: IdentityUser
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Favorites = new HashSet<Film>();
        }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Film> Favorites { get; set; }
    }
}
