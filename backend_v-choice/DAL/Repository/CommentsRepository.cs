using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly DBContext _context;

        public CommentsRepository(DBContext dbc)
        {
            _context = dbc;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment, string userId)
        {
            User author = await _context.User.FirstOrDefaultAsync(c => c.Id == userId);

            comment.Author = author;
            comment.AuthorId = author.Id;
            comment.AuthorEmail = author.Email;
            comment.CreatedAt = DateTime.Now;

            var film = _context.Film.Find(comment.FilmId);
            if (film != null)
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
            Comment item = _context.Comment.Find(id);
            _context.Comment.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(int id, Comment comment)
        {
            Comment item = _context.Comment.Find(id);
            item.Text = comment.Text;
            _context.Comment.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
