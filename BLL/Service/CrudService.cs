using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CrudService : ICrudService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly ILogger _logger;
        private readonly IMapperDTOtoModel _mapper;

        public CrudService(IFilmRepository fr, IGenreRepository gr, ICommentsRepository cr, ILogger<CrudService> logger, IMapperDTOtoModel mapper)
        {
            _filmRepository = fr;
            _genreRepository = gr;
            _commentsRepository = cr;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user)
        {
            _logger.LogInformation("Start creating comment.");
            try
            {
                Comment c = _mapper.CommentDTOtoModel(comment);
                
                _logger.LogInformation("Call CreateCommentAsync.");
                c = await _commentsRepository.CreateCommentAsync(c, user);
                _logger.LogInformation($"Create comment: comment with Id equal {comment.Id} was created.");
                _logger.LogInformation("Convert to DTO.");

                return new CommentDTO(c);
            }
            catch (Exception e)
            {
                _logger.LogError($"CreateCommentAsync has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<FilmDTO> CreateFilmAsync(FilmDTO film)
        {
            _logger.LogInformation("Start creating film.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);
                
                _logger.LogInformation("Call CreateFilmAsync.");
                f = await _filmRepository.CreateFilmAsync(f);
                _logger.LogInformation($"Create film: film with Id equal {film.Id} was created.");
                _logger.LogInformation("Convert to DTO.");
                
                return new FilmDTO(f);
            }
            catch(Exception e)
            {
                _logger.LogError($"CreateFilmAsync has thrown an exception: {e.Message}.");
                
                return null;
            }
        }

        public async Task DeleteCommentAsync(int id)
        {
            _logger.LogInformation($"Start deleting comment with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteCommentAsync.");
                await _commentsRepository.DeleteCommentAsync(id);
                _logger.LogInformation($"Delete comment: comment with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete comment with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task DeleteFilmAsync(int id)
        {
            _logger.LogInformation($"Start deleting film with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteFilmAsync.");
                await _filmRepository.DeleteFilmAsync(id);
                _logger.LogInformation($"Delete film: film with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public object GetAllCommentsAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<FilmDTO> GetAllFilms()
        {
            _logger.LogInformation($"Starting get all films.");
            try
            {
                _logger.LogInformation("Call GetAllFilms.");
                IEnumerable<Film> films = _filmRepository.GetAllFilms();
                _logger.LogInformation("Get all films successfull.");
                _logger.LogInformation("Convert to DTO.");
                
                return films.Select(e => new FilmDTO(e)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAllFilms has thrown an exception: {e.Message}.");
                
                return new List<FilmDTO>();
            }
        }

        public IEnumerable<GenreDTO> GetAllGenres()
        {
            _logger.LogInformation($"Starting get all genres.");
            try
            {
                _logger.LogInformation("Call GetAllGenres.");
                IEnumerable<Genre> genres = _genreRepository.GetAllGenres();
                _logger.LogInformation("Get all genres successfull.");
                _logger.LogInformation("Convert to DTO.");

                return genres.Select(e => new GenreDTO(e)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAllGenres has thrown an exception: {e.Message}.");

                return new List<GenreDTO>();
            }
        }

        public Task<FilmDTO> GetFilmAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<FilmDTO>> GetFilmsByGenreIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateCommentAsync(int id, CommentDTO comment)
        {
            _logger.LogInformation($"Start updating film with Id equal {id}.");
            try
            {
                Comment c = _mapper.CommentDTOtoModel(comment);

                _logger.LogInformation("Call UpdateFilmAsync.");
                await _commentsRepository.UpdateCommentAsync(id, c);
                _logger.LogInformation($"Update comment: comment with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update comment with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task UpdateFilmAsync(int id, FilmDTO film)
        {
            _logger.LogInformation($"Start updating film with Id equal {id}.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);
                
                _logger.LogInformation("Call UpdateFilmAsync.");
                await _filmRepository.UpdateFilmAsync(id, f);
                _logger.LogInformation($"Update film: film with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update film with id={id} has thrown an exception: {e.Message}.");
            }
        }
    }
}
