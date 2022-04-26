using DAL.Model;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICommentsRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment, string userId);
        Task UpdateCommentAsync(int id, Comment comment);
        Task DeleteCommentAsync(int id);
        Task<Pagination<Comment>> GetByDateOnlyAsync(int pageNumber, int onPageCount, int filmId);
        Task<Pagination<Comment>> GetByDateDescendingOnlyAsync(int pageNumber, int onPageCount, int filmId);
        Task<Pagination<Comment>> GetByDateUserFirstAsync(int pageNumber, int onPageCount, int filmId, string userId);
        Task<Pagination<Comment>> GetByDateDescendingUserFirstAsync(int pageNumber, int onPageCount, int filmId, string userId);
    }
}
