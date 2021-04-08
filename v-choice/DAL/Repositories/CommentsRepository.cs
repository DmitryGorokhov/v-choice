using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Interfaces;
using v_choice.Models;

namespace v_choice.DAL.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly DBContext _context;
        private readonly IUserRepository _users;
        public CommentsRepository(DBContext dbc, IUserRepository users)
        {
            _context = dbc;
            _users = users;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment, System.Security.Claims.ClaimsPrincipal user)
        {
            User author = await _users.GetCurrentUserAsync(user);
            comment.Author = author;
            comment.AuthorId = author.Id;
            comment.CreatedAt = new DateTime().Date;

            var film = _context.Film.Find(comment.FilmId);
            if(film != null)
            {
                film.Comments.Add(comment);
                _context.Film.Update(film);
            }
            author.Comments.Add(comment);
            _context.User.Update(author);

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var item = _context.Comment.Find(id);
            _context.Comment.Remove(item);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Comment> GetAllCommentsAsync(int id)
        {
            return _context.Comment.Where(e => e.FilmId == id);
        }

        public async Task UpdateCommentAsync(int id, Comment comment)
        {
            var item = _context.Comment.Find(id);
            item.Text = comment.Text;
            _context.Comment.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
