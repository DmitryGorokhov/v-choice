using System.Collections.Generic;
using System.Linq;
using DAL.Model;

namespace BLL.DTO
{
    public class FilmDTO
    {
        public FilmDTO()
        { }

        public FilmDTO(Film film)
        {
            Id = film.Id;
            Title = film.Title;
            Year = film.Year;
            Description = film.Description;
            Comments = new HashSet<CommentDTO>(film.Comments.Select(e => new CommentDTO(e)).ToList());
            Genres = new HashSet<GenreDTO>(film.Genres.Select(e => new GenreDTO(e)).ToList());
            Users = new HashSet<UserDTO>(film.Users.Select(e => new UserDTO(e)).ToList());
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string Description { get; set; }

        public ICollection<CommentDTO> Comments { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }

        public ICollection<UserDTO> Users { get; set; }
    }
}
