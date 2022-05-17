using DAL.Model;

namespace BLL.DTO
{
    public class ParticipationDTO
    {
        public int? Id { get; set; }
        public int FilmId { get; set; }
        public int PersonId { get; set; }
        public PersonDTO Person { get; set; }
        public RoleEnum RoleId { get; set; }
        public string Role { get; set; }
    }
}