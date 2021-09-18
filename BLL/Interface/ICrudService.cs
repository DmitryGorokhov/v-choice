using BLL.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICrudService
    {
        IEnumerable<FilmDTO> GetAllFilms();
        Task<FilmDTO> GetFilmAsync(int id);
        Task<FilmDTO> CreateFilmAsync(FilmDTO film);
        Task UpdateFilmAsync(int id, FilmDTO film);
        Task DeleteFilmAsync(int id);
        object GetAllCommentsAsync(int id);
        Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, CommentDTO comment);
        Task DeleteCommentAsync(int id);
        IEnumerable<GenreDTO> GetAllGenres();
        Task<ICollection<FilmDTO>> GetFilmsByGenreIdAsync(int id);
    }
}
