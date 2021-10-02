using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DAL.Model
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
