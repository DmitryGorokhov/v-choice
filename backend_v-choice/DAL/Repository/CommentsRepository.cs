using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Interface;
using DAL.Model;

namespace DAL.Repository
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
            comment.AuthorEmail = author.Email;
            comment.CreatedAt = DateTime.Now.Date;

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
            try
            {
                Comment item = _context.Comment.Find(id);
                _context.Comment.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Pagination<Comment>> GetCommentsByPageAsync(int pageNumber, int onPageCount)
        {
            int total = await _context.Comment.CountAsync();
            var items = await _context.Comment.Skip((pageNumber - 1) * onPageCount).Take(onPageCount).ToListAsync();

            return new Pagination<Comment>()
            {
                Items = items,
                TotalCount = total
            };
        }

        public async Task UpdateCommentAsync(int id, Comment comment)
        {
            try
            {
                Comment item = _context.Comment.Find(id);
                item.Text = comment.Text;
                _context.Comment.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
