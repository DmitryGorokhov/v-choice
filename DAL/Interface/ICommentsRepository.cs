using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Model;

namespace DAL.Interface
{
    public interface ICommentsRepository
    {
        IEnumerable<Comment> GetAllCommentsAsync(int id);
        Task<Comment> CreateCommentAsync(Comment comment, System.Security.Claims.ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, Comment comment);
        Task DeleteCommentAsync(int id);
        Task<Pagination<Comment>> GetCommentsByPageAsync(int pageNumber, int onPageCount);
    }
}
