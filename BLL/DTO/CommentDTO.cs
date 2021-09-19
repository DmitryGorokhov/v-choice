using System;
using DAL.Model;

namespace BLL.DTO
{
    public class CommentDTO
    {
        public CommentDTO()
        { }

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            Text = comment.Text;
            CreatedAt = comment.CreatedAt;
            AuthorId = comment.AuthorId;
            AuthorEmail = comment.AuthorEmail;
            FilmId = comment.FilmId;
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorId { get; set; }

        public string AuthorEmail { get; set; }

        public int FilmId { get; set; }
    }
}
