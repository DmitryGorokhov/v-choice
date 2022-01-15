using System.Collections.Generic;

namespace DAL.Model
{
    public class Film
    {
        public Film()
        {
            Comments = new HashSet<Comment>();
            Users = new HashSet<User>();
            Genres = new HashSet<Genre>();
            RateCollection = new HashSet<Rate>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public int TotalRate { get; set; }
        public float AverageRate { get; set; }
        public int CountRate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Rate> RateCollection { get; set; }
    }
}
