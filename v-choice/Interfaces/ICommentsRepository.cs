using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Interfaces
{
    public interface ICommentsRepository
    {
        IEnumerable<Comment> GetAllCommentsAsync(int id);
        Task<Comment> CreateCommentAsync(Comment comment, System.Security.Claims.ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, Comment comment);
        Task DeleteCommentAsync(int id);
    }
}
