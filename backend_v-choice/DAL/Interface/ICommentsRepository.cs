using DAL.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICommentsRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment, ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, Comment comment);
        Task DeleteCommentAsync(int id);
        Task<Pagination<Comment>> GetCommentsByPageAsync(int pageNumber, int onPageCount, int? filmId, bool commonOrder, bool userFirst, ClaimsPrincipal user);
    }
}
