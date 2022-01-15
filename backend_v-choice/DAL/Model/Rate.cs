namespace DAL.Model
{
    public class Rate
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string AuthorId { get; set; }
        public string AuthorEmail { get; set; }
        public int FilmId { get; set; }
        public virtual User Author { get; set; }
        public virtual Film Film { get; set; }
    }
}
