using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IPaginationService
    {
        Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQuery query);
        Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQuery query);
        Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQuery query);
    }
}
