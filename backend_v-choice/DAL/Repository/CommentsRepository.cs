using DAL.Interface;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public async Task<Pagination<Comment>> GetByDateOnlyAsync(int pageNumber, int onPageCount, int? filmId)
        {
            var collection = _context.Comment
                .Where(c => c.FilmId == filmId)
                .OrderBy(c => c.CreatedAt);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Comment>> GetByDateDescendingOnlyAsync(int pageNumber, int onPageCount, int? filmId)
        {
            var collection = _context.Comment
                .Where(c => c.FilmId == filmId)
                .OrderByDescending(c => c.CreatedAt);

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Comment>> GetByDateUserFirstAsync(int pageNumber, int onPageCount, int? filmId, string userId)
        {
            var collection = _context.Comment.Where(c => c.FilmId == filmId);

            collection = collection
                .Where(c => c.AuthorId == userId)
                .OrderBy(c => c.CreatedAt)
                .Union(collection
                        .Where(c => c.AuthorId != userId)
                        .OrderBy(c => c.CreatedAt));

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        public async Task<Pagination<Comment>> GetByDateDescendingUserFirstAsync(int pageNumber, int onPageCount, int? filmId, string userId)
        {
            var collection = _context.Comment.Where(c => c.FilmId == filmId);

            collection = collection
                .Where(c => c.AuthorId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .Union(collection
                        .Where(c => c.AuthorId != userId)
                        .OrderByDescending(c => c.CreatedAt));

            return await SplitByPagesAsync(collection, pageNumber, onPageCount);
        }

        private async Task<Pagination<Comment>> SplitByPagesAsync(IQueryable<Comment> collection, int pageNumber, int onPageCount)
        {
            var total = await collection.CountAsync();
            var items = await collection.Skip((pageNumber - 1) * onPageCount).Take(onPageCount).ToListAsync();

            return new Pagination<Comment>()
            {
                Items = items,
                TotalCount = total
            };
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
