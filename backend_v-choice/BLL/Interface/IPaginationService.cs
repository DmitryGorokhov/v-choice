using BLL.DTO;
using BLL.Query;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IPaginationService
    {
        Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQueryFilms query);
        Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQueryFavorites query, ClaimsPrincipal user);
        Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQueryComments query, ClaimsPrincipal user);
        Task<PaginationDTO<PersonDTO>> GetPersonsPaginationAsync(PaginationQueryBase query);
    }
}
