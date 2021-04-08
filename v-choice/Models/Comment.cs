using System;

namespace v_choice.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
        public virtual User Author { get; set; }
    }
}
