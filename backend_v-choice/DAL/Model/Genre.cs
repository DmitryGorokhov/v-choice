using System.Collections.Generic;

namespace DAL.Model
{
    public class Genre
    {
        public Genre() : base()
        {
            Films = new HashSet<Film>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public int Requested { get; set; }

        public virtual ICollection<Film> Films { get; set; }
    }
}
