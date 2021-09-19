using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;

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
                
                _logger.LogInformation($"Create comment: comment with Id equal {comment.Id} was created. Convert to DTO before return.");

                return new CommentDTO(c);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create comment has thrown an exception: {e.Message}.");

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
                
                _logger.LogInformation($"Create film: film with Id equal {film.Id} was created. Convert to DTO before return.");
                
                return new FilmDTO(f);
            }
            catch(Exception e)
            {
                _logger.LogError($"Create film has thrown an exception: {e.Message}.");
                
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

        public IEnumerable<GenreDTO> GetAllGenres()
        {
            _logger.LogInformation("Starting get all genres.");
            try
            {
                _logger.LogInformation("Call GetAllGenres.");
                var genres = _genreRepository.GetAllGenres();

                _logger.LogInformation("Get all genres successfull. Convert to DTO before return.");

                return genres.Select(e => new GenreDTO(e)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Get all genres has thrown an exception: {e.Message}.");

                return new List<GenreDTO>();
            }
        }

        public async Task<FilmDTO> GetFilmAsync(int id)
        {
            _logger.LogInformation($"Starting get film with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call GetFilmAsync.");
                var film = await _filmRepository.GetFilmAsync(id);

                if (film == null)
                {
                    _logger.LogInformation($"Get film: film with Id equal {id} not found.");
                    
                    return null;
                }

                _logger.LogInformation($"Get film with Id equal {id} successfully. Convert to DTO before return.");

                return new FilmDTO(film);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get film with Id equal {id} has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UpdateCommentAsync(int id, CommentDTO comment)
        {
            _logger.LogInformation($"Start updating film with Id equal {id}.");
            try
            {
                Comment c = _mapper.CommentDTOtoModel(comment);

                _logger.LogInformation("Call UpdateCommentAsync.");
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
