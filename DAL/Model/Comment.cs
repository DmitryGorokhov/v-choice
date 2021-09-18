using System;

namespace DAL.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public string AuthorEmail { get; set; }
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
        public virtual User Author { get; set; }
    }
}
