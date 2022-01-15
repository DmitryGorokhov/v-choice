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
            RateCollection = new HashSet<Rate>();
        }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Film> Favorites { get; set; }
        public virtual ICollection<Rate> RateCollection { get; set; }
    }
}
