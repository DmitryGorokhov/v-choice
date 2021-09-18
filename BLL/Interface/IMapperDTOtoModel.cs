using BLL.DTO;
using DAL.Model;

namespace BLL.Interface
{
    public interface IMapperDTOtoModel
    {
        Film FilmDTOtoModel(FilmDTO film);
        Genre GenreDTOtoModel(GenreDTO genre);
        User UserDTOtoModel(UserDTO user);
        Comment CommentDTOtoModel(CommentDTO comment);
    }
}
