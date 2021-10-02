using System.Collections.Generic;

namespace DAL.Model
{
    public class Film
    {
        public Film() : base()
        {
            Comments = new HashSet<Comment>();
            Users = new HashSet<User>();
            Genres = new HashSet<Genre>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
