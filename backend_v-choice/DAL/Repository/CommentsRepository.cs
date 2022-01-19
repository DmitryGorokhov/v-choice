using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            try
            {
                Comment item = _context.Comment.Find(id);
                _context.Comment.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Pagination<Comment>> GetCommentsByPageAsync(int pageNumber, int onPageCount, int? filmId, bool commonOrder, bool userFirst, ClaimsPrincipal user)
        {
            if (filmId is null)
            {
                // All comments.
                return GetCommentsByPage(pageNumber, onPageCount);
            }

            // Get by film Id.
            var collection = _context.Comment.Where(c => c.FilmId == filmId).Select(c => c);

            if (userFirst)
            {
                string userId = (await _users.GetCurrentUserAsync(user)).Id;

                // By userId first and by date order.
                collection = commonOrder switch
                {
                    // old to new
                    false => collection.Where(c => c.AuthorId == userId).OrderBy(c => c.CreatedAt)
                        .Union(collection.Where(c => c.AuthorId != userId).OrderBy(c => c.CreatedAt)),
                    // new to old, default
                    true => collection.Where(c => c.AuthorId == userId).OrderByDescending(c => c.CreatedAt)
                        .Union(collection.Where(c => c.AuthorId != userId).OrderByDescending(c => c.CreatedAt)),
                };
            }
            else
            {
                // By date order only.
                collection = commonOrder switch
                {
                    // old to new
                    false => collection.OrderBy(c => c.CreatedAt),
                    // new to old, default
                    true => collection.OrderByDescending(c => c.CreatedAt),
                };
            }

            var total = await collection.CountAsync();
            var items = await collection.Skip((pageNumber - 1) * onPageCount).Take(onPageCount).ToListAsync();

            return new Pagination<Comment>()
            {
                Items = items,
                TotalCount = total
            };
        }

        private static Pagination<Comment> GetCommentsByPage(int pageNumber, int onPageCount)
        {
            // Is not allowed. Returns empty.
            return new Pagination<Comment>();
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
            catch
            {
                throw;
            }
        }
    }
}
