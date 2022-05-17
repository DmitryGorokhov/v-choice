namespace DAL.Model
{
    public class Participation
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int PersonId { get; set; }
        public Film Film { get; set; }
        public Person Person { get; set; }
        public RoleEnum Role { get; set; }
    }
}
