using DAL.Model;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICommentsRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment, string userId);
        Task UpdateCommentAsync(int id, Comment comment);
        Task DeleteCommentAsync(int id);
    }
}
