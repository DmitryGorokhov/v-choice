using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interface
{
    public interface ICrudService
    {
        Task<FilmDTO> GetFilmAsync(int id);
        Task<FilmDTO> CreateFilmAsync(FilmDTO film);
        Task UpdateFilmAsync(int id, FilmDTO film);
        Task DeleteFilmAsync(int id);
        Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, CommentDTO comment);
        Task DeleteCommentAsync(int id);
        IEnumerable<GenreDTO> GetAllGenres();
    }
}
