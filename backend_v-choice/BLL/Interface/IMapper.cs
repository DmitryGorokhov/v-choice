using BLL.DTO;
using DAL.Model;

namespace BLL.Interface
{
    public interface IMapper
    {
        Film FilmDTOtoModel(FilmDTO film);
        FilmDTO FilmModelToDTO(Film film);

        Genre GenreDTOtoModel(GenreDTO genre);
        GenreDTO GenreModelToDTO(Genre genre);

        User UserDTOtoModel(UserDTO user);
        UserDTO UserModelToDTO(User user);

        Comment CommentDTOtoModel(CommentDTO comment);
        CommentDTO CommentModelToDTO(Comment comment);

        Rate RateDTOtoModel(RateDTO rate);
        RateDTO RateModelToDTO(Rate rate);

        GenreStatisticDTO GenreModelToStatisticDTO(Genre genre);
        FilmStatisticDTO FilmModelToStatisticDTO(Film film);

        Participation ParticipationDTOtoModel(ParticipationDTO participation);
        ParticipationDTO ParticipationModelToDTO(Participation participation);

        Person PersonDTOtoModel(PersonDTO person);
        PersonDTO PersonModelToDTO(Person person);

        Studio StudioDTOtoModel(StudioDTO studio);
        StudioDTO StudioModelToDTO(Studio studio);
    }
}
