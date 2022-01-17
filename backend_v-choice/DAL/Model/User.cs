using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DAL.Model
{
    public partial class User: IdentityUser
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Favorites = new HashSet<Favorite>();
            RateCollection = new HashSet<Rate>();
        }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Rate> RateCollection { get; set; }

        public static implicit operator string(User v)
        {
            throw new NotImplementedException();
        }
    }
}
