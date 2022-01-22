using BLL.DTO;
using BLL.Interface;
using DAL.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Utils
{
    public class Mapper: IMapper
    {
        private readonly ILogger _logger;

        public Mapper(ILogger<Mapper> logger)
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
                AverageRate = film.AverageRate,
                CountRate = film.CountRate,
                TotalRate = film.TotalRate,
                CreatedAt = film.CreatedAt,

                Comments = new HashSet<Comment>(film.Comments
                .Select(e => CommentDTOtoModel(e))
                .ToList()),

                Genres = new HashSet<Genre>(film.Genres
                .Select(e => GenreDTOtoModel(e))
                .ToList()),

                InFavorites = new HashSet<Favorite>(film.InFavorites
                .Select(e => FavoriteDTOtoModel(e))
                .ToList()),

                RateCollection = new HashSet<Rate>(film.RateCollection
                .Select(e => RateDTOtoModel(e))
                .ToList())
            };

            _logger.LogInformation("Finish mapping FilmDTO to Model.");

            return model;
        }

        public Rate RateDTOtoModel(RateDTO rate)
        {
            _logger.LogInformation("Start mapping RateDTO to Model.");
            Rate model = new Rate()
            {
                Id = rate.Id,
                Value = rate.Value,
                AuthorId = rate.AuthorId,
                AuthorEmail = rate.AuthorEmail,
                FilmId = rate.FilmId
            };

            _logger.LogInformation("Finish mapping RateDTO to Model.");

            return model;
        }

        public Favorite FavoriteDTOtoModel(FavoriteDTO fav)
        {
            _logger.LogInformation("Start mapping FavoriteDTO to Model.");
            Favorite model = new Favorite()
            {
                Id = fav.Id,
                AddedAt = fav.AddedAt,
                AuthorId = fav.AuthorId,
                FilmId = fav.FilmId
            };

            _logger.LogInformation("Finish mapping FavoriteDTO to Model.");

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
                Id = user.Id,
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

        public FilmDTO FilmModelToDTO(Film film)
        {
            _logger.LogInformation("Convert to DTO before return.");
            if (film == null)
            {
                _logger.LogInformation("Model instance is null. Skip creating DTO.");

                return null;
            }

            return new FilmDTO() 
            {
                Id = film.Id,
                Title = film.Title,
                Year = film.Year,
                Description = film.Description,
                AverageRate = film.AverageRate,
                CountRate = film.CountRate,
                TotalRate = film.TotalRate,
                CreatedAt = film.CreatedAt,

                Comments = new HashSet<CommentDTO>(film.Comments.Select(e => CommentModelToDTO(e)).ToList()),
                Genres = new HashSet<GenreDTO>(film.Genres.Select(e => GenreModelToDTO(e)).ToList()),
                InFavorites = new HashSet<FavoriteDTO>(film.InFavorites.Select(e => FavoriteModelToDTO(e)).ToList()),
                RateCollection = new HashSet<RateDTO>(film.RateCollection.Select(e => RateModelToDTO(e)).ToList()),
            };
        }

        public RateDTO RateModelToDTO(Rate rate)
        {
            _logger.LogInformation("Convert to DTO before return.");
            if (rate == null)
            {
                _logger.LogInformation("Model instance is null. Skip creating DTO.");

                return null;
            }

            return new RateDTO()
            {
                Id = rate.Id,
                Value = rate.Value,
                AuthorId = rate.AuthorId,
                AuthorEmail = rate.AuthorEmail,
                FilmId = rate.FilmId
            };
        }

        public FavoriteDTO FavoriteModelToDTO(Favorite fav)
        {
            _logger.LogInformation("Convert to DTO before return.");
            if (fav == null)
            {
                _logger.LogInformation("Model instance is null. Skip creating DTO.");

                return null;
            }

            return new FavoriteDTO()
            {
                Id = fav.Id,
                AddedAt = fav.AddedAt,
                AuthorId = fav.AuthorId,
                FilmId = fav.FilmId
            };
        }

        public GenreDTO GenreModelToDTO(Genre genre)
        {
            _logger.LogInformation("Convert to DTO before return.");
            if (genre == null)
            {
                _logger.LogInformation("Model instance is null. Skip creating DTO.");

                return null;
            }

            return new GenreDTO()
            {
                Id = genre.Id,
                Value = genre.Value
            };
        }

        public UserDTO UserModelToDTO(User user)
        {
            _logger.LogInformation("Convert to DTO before return.");
            if (user == null)
            {
                _logger.LogInformation("Model instance is null. Skip creating DTO.");

                return null;
            }

            return new UserDTO()
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public CommentDTO CommentModelToDTO(Comment comment)
        {
            _logger.LogInformation("Convert to DTO before return.");
            if (comment == null)
            {
                _logger.LogInformation("Model instance is null. Skip creating DTO.");

                return null;
            }

            return new CommentDTO()
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                AuthorId = comment.AuthorId,
                AuthorEmail = comment.AuthorEmail,
                FilmId = comment.FilmId
            };
        }
    }
}
