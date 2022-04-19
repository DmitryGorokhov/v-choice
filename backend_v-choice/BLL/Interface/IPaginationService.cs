using BLL.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IPaginationService
    {
        Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQuery query);
        Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQuery query, ClaimsPrincipal user);
        Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQuery query, ClaimsPrincipal user);
    }
}
