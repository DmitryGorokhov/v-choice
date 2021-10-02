using BLL.DTO;
using DAL.Model;

namespace BLL.Interface
{
    public interface IMapper
    {
        Film FilmDTOtoModel(FilmDTO film);
        Genre GenreDTOtoModel(GenreDTO genre);
        User UserDTOtoModel(UserDTO user);
        Comment CommentDTOtoModel(CommentDTO comment);
        FilmDTO FilmModelToDTO(Film film);
        GenreDTO GenreModelToDTO(Genre genre);
        UserDTO UserModelToDTO(User user);
        CommentDTO CommentModelToDTO(Comment comment);
    }
}
