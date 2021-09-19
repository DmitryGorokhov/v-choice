using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using DAL.Model;

namespace BLL.Utils
{
    public class MapperDTOtoModel: IMapperDTOtoModel
    {
        private readonly ILogger _logger;

        public MapperDTOtoModel(ILogger<MapperDTOtoModel> logger)
        {
            _logger = logger;
        }

        public Film FilmDTOtoModel(FilmDTO film)
        {
            _logger.LogInformation("Start mapping FilmDTO to Model.");
            Film model = new Film()
            {
                Id = film.Id,
                Title = film.Title,
                Year = film.Year,
                Description = film.Description,
                Comments = new HashSet<Comment>(film.Comments.Select(e => CommentDTOtoModel(e)).ToList()),
                Genres = new HashSet<Genre>(film.Genres.Select(e => GenreDTOtoModel(e)).ToList()),
                Users = new HashSet<User>(film.Users.Select(e => UserDTOtoModel(e)).ToList())
            };

            _logger.LogInformation("Finish mapping FilmDTO to Model.");

            return model;
        }

        public Genre GenreDTOtoModel(GenreDTO genre)
        {
            _logger.LogInformation("Start mapping GenreDTO to Model.");
            Genre model = new Genre()
            {
                Id = genre.Id,
                Value = genre.Value
            };

            _logger.LogInformation("Finish mapping GenreDTO to Model.");

            return model;
        }

        public User UserDTOtoModel(UserDTO user)
        {
            _logger.LogInformation("Start mapping UserDTO to Model.");
            User model = new User()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            _logger.LogInformation("Finish mapping UserDTO to Model.");

            return model;
        }

        public Comment CommentDTOtoModel(CommentDTO comment)
        {
            _logger.LogInformation("Start mapping CommentDTO to Model.");
            Comment model = new Comment()
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                AuthorId = comment.AuthorId,
                AuthorEmail = comment.AuthorEmail,
                FilmId = comment.FilmId
            };

            _logger.LogInformation("Finish mapping CommentDTO to Model.");

            return model;
        }
    }
}
