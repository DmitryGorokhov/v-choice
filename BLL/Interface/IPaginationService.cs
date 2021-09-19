using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interface
{
    public interface IPaginationService
    {
        Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQuery query);
        Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQuery query);
        Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQuery query);
    }
}
