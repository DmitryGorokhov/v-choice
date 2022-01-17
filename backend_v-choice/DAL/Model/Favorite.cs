using System;

namespace DAL.Model
{
    public class Favorite
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public string AuthorId { get; set; }
        public int FilmId { get; set; }
        public User Author { get; set; }
        public Film Film { get; set; }
    }
}
