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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Service
{
    public class CrudService : ICrudService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IRateRepository _rateRepository;
        private readonly IAutorizationService _autorizationService;
        private readonly IPersonRepository _personRepository;
        private readonly IStudioRepository _studioRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CrudService(IFilmRepository fr, IGenreRepository gr, ICommentsRepository cr, ILogger<CrudService> logger, IRateRepository rr, IAutorizationService aus, IPersonRepository pr, IStudioRepository sr, IMapper mapper)
        {
            _filmRepository = fr;
            _genreRepository = gr;
            _commentsRepository = cr;
            _logger = logger;
            _rateRepository = rr;
            _autorizationService = aus;
            _personRepository = pr;
            _studioRepository = sr;
            _mapper = mapper;
        }

        public async Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user)
        {
            _logger.LogInformation("Start creating comment.");
            try
            {
                Comment c = _mapper.CommentDTOtoModel(comment);

                _logger.LogInformation("Call CreateCommentAsync.");

                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;
                c = await _commentsRepository.CreateCommentAsync(c, userId);

                _logger.LogInformation($"Create comment: comment with Id equal {comment.Id} was created.");

                return _mapper.CommentModelToDTO(c);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create comment has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<FilmDTO> CreateFilmAsync(FilmDTO film, IWebHostEnvironment _appEnvironment)
        {
            _logger.LogInformation("Start creating film.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);

                _logger.LogInformation("Call CreateFilmAsync.");
                f = await _filmRepository.CreateFilmAsync(f);

                _logger.LogInformation($"Create film: film with Id equal {f.Id} was created.");

                string posterPath = await SaveImageAsync(film.Poster, _appEnvironment);

                if (posterPath != null)
                {
                    _logger.LogInformation("Call SetPosterPathAsync.");
                    f = await _filmRepository.SetPosterPathAsync(f.Id, posterPath);
                }

                return _mapper.FilmModelToDTO(f);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create film has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<GenreDTO> CreateGenreAsync(GenreDTO genre)
        {
            _logger.LogInformation("Start creating genre.");
            try
            {
                Genre g = _mapper.GenreDTOtoModel(genre);

                _logger.LogInformation("Call CreateGenreAsync.");
                g = await _genreRepository.CreateGenreAsync(g);

                _logger.LogInformation($"Create genre: genre with Id equal {g.Id} was created.");

                return _mapper.GenreModelToDTO(g);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create genre has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<RateDTO> CreateRateAsync(RateDTO rate, ClaimsPrincipal user)
        {
            _logger.LogInformation("Start creating rate.");
            try
            {
                Rate r = _mapper.RateDTOtoModel(rate);

                _logger.LogInformation("Call CreateRateAsync.");
                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;
                r = await _rateRepository.CreateRateAsync(r, userId);

                _logger.LogInformation($"Create rate: rate with Id equal {rate.Id} was created.");

                return _mapper.RateModelToDTO(r);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create rate has thrown an exception: {e.Message}.");

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

        public async Task DeleteGenreAsync(int id)
        {
            _logger.LogInformation($"Start deleting genre with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteGenreAsync.");
                await _genreRepository.DeleteGenreAsync(id);

                _logger.LogInformation($"Delete genre: genre with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete genre with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task DeleteRateAsync(int id)
        {
            _logger.LogInformation($"Start deleting rate with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteRateAsync.");
                await _rateRepository.DeleteRateAsync(id);

                _logger.LogInformation($"Delete rate: rate with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete rate with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public IEnumerable<GenreDTO> GetAllGenres()
        {
            _logger.LogInformation("Starting get all genres.");
            try
            {
                _logger.LogInformation("Call GetAllGenres.");
                var genres = _genreRepository.GetAllGenres();

                _logger.LogInformation("Get all genres successfull.");

                return genres.Select(e => _mapper.GenreModelToDTO(e)).ToList();
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

                _logger.LogInformation($"Get film with Id equal {id} successfully.");

                _logger.LogInformation($"Write that film with Id equal {id} was requested. Call FilmRequestedCounter.");
                await _filmRepository.FilmRequestedCounter(film.Id);


                return _mapper.FilmModelToDTO(film);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get film with Id equal {id} has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<RateDTO> GetFilmRate(int filmId, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start get film rate with id={filmId} of current user.");
            try
            {
                _logger.LogInformation("Call GetFilmRate.");
                User currentUser = await _autorizationService.GetCurrentUserModelAsync(user);
                if (currentUser == null)
                {
                    _logger.LogInformation("Get film rate of current user: no user.");

                    return null;
                }

                var res = await _rateRepository.GetFilmRate(filmId, currentUser.Id);
                if (res == null)
                {
                    _logger.LogInformation($"Get film rate of current user: rate of film with Id equal {filmId} not found.");

                    return null;
                }

                _logger.LogInformation($"Get film rate of current user: Ok - {res.Value}.");

                return _mapper.RateModelToDTO(res);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get rate of film with id={filmId} of current user has thrown an exception: {e.Message}.");

                return null;
            }
        }

        private async Task<string> SaveImageAsync(IFormFile image, IWebHostEnvironment _appEnvironment)
        {
            _logger.LogInformation("Start saving image.");
            try
            {
                if (image == null)
                {
                    _logger.LogInformation("Save image: image is null.");

                    return null;
                }

                string imageName = new string(Path.GetFileNameWithoutExtension(image.FileName).Take(20).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(image.FileName);

                string path = Path.Combine("img", imageName);

                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                _logger.LogInformation($"Save image: Ok - path: {path}.");

                return path;
            }
            catch (Exception e)
            {
                _logger.LogError($"Save image has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UpdateCommentAsync(int id, CommentDTO comment)
        {
            _logger.LogInformation($"Start updating comment with Id equal {id}.");
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

        public async Task<string> UpdateFilmAsync(int id, FilmDTO film, IWebHostEnvironment _appEnvironment)
        {
            _logger.LogInformation($"Start updating film with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call SavePosterAsync.");
                string newPath = await SaveImageAsync(film.Poster, _appEnvironment);

                if (newPath != null)
                {
                    film.PosterPath = newPath;
                }

                Film f = _mapper.FilmDTOtoModel(film);

                _logger.LogInformation("Call UpdateFilmAsync.");
                await _filmRepository.UpdateFilmAsync(id, f);

                _logger.LogInformation($"Update film: film with Id equal {id} was updated.");

                return newPath;
            }
            catch (Exception e)
            {
                _logger.LogError($"Update film with id={id} has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UpdateGenreAsync(int id, GenreDTO genre)
        {
            _logger.LogInformation($"Start updating genre with Id equal {id}.");
            try
            {
                Genre g = _mapper.GenreDTOtoModel(genre);

                _logger.LogInformation("Call UpdateGenreAsync.");
                await _genreRepository.UpdateGenreAsync(id, g);

                _logger.LogInformation($"Update genre: genre with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update genre with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task UpdateRateAsync(int id, RateDTO rate)
        {
            _logger.LogInformation($"Start updating rate with Id equal {id}.");
            try
            {
                Rate r = _mapper.RateDTOtoModel(rate);

                _logger.LogInformation("Call UpdateRateAsync.");
                await _rateRepository.UpdateRateAsync(id, r);

                _logger.LogInformation($"Update rate: rate with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update rate with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task<PersonDTO> CreatePersonAsync(PersonDTO person, IWebHostEnvironment appEnvironment)
        {
            _logger.LogInformation("Start creating person.");
            try
            {
                Person p = _mapper.PersonDTOtoModel(person);

                _logger.LogInformation("Call CreatePersonAsync.");
                p = await _personRepository.CreatePersonAsync(p);

                _logger.LogInformation($"Create person: person with Id equal {p.Id} was created.");

                string photoPath = await SaveImageAsync(person.Photo, appEnvironment);

                if (photoPath != null)
                {
                    _logger.LogInformation("Call SetPhotoPathAsync.");
                    p = await _personRepository.SetPhotoPathAsync(p.Id, photoPath);
                }

                return _mapper.PersonModelToDTO(p);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create person has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<string> UpdatePersonAsync(int id, PersonDTO person, IWebHostEnvironment appEnvironment)
        {
            _logger.LogInformation($"Start updating person with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call SavePosterAsync.");
                string newPath = await SaveImageAsync(person.Photo, appEnvironment);

                if (newPath != null)
                {
                    person.PhotoPath = newPath;
                }

                Person p = _mapper.PersonDTOtoModel(person);

                _logger.LogInformation("Call UpdatePersonAsync.");
                await _personRepository.UpdatePersonAsync(id, p);

                _logger.LogInformation($"Update person: person with Id equal {id} was updated.");

                return newPath;
            }
            catch (Exception e)
            {
                _logger.LogError($"Update person with id={id} has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task DeletePersonAsync(int id)
        {
            _logger.LogInformation($"Start deleting person with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeletePersonAsync.");
                await _personRepository.DeletePersonAsync(id);

                _logger.LogInformation($"Delete person: film with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete person with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public IEnumerable<StudioDTO> GetAllStudios()
        {
            _logger.LogInformation("Starting get all studios.");
            try
            {
                _logger.LogInformation("Call GetAllStudios.");
                IQueryable<Studio> studios = _studioRepository.GetAllStudios();

                _logger.LogInformation("Get all studios successfull.");

                return studios.Select(e => _mapper.StudioModelToDTO(e)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Get all studios has thrown an exception: {e.Message}.");

                return new List<StudioDTO>();
            }
        }

        public async Task<StudioDTO> CreateStudioAsync(StudioDTO studio)
        {
            _logger.LogInformation("Start creating studio.");
            try
            {
                Studio s = _mapper.StudioDTOtoModel(studio);

                _logger.LogInformation("Call CreateStudioAsync.");
                s = await _studioRepository.CreateStudioAsync(s);

                _logger.LogInformation($"Create studio: studio with Id equal {s.Id} was created.");

                return _mapper.StudioModelToDTO(s);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create studio has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UpdateStudioAsync(int id, StudioDTO studio)
        {
            _logger.LogInformation($"Start updating studio with Id equal {id}.");
            try
            {
                Studio s = _mapper.StudioDTOtoModel(studio);

                _logger.LogInformation("Call UpdateStudioAsync.");
                await _studioRepository.UpdateStudioAsync(id, s);

                _logger.LogInformation($"Update studio: studio with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update studio with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task DeleteStudioAsync(int id)
        {
            _logger.LogInformation($"Start deleting studio with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteStudioAsync.");
                await _studioRepository.DeleteStudioAsync(id);

                _logger.LogInformation($"Delete studio: studio with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete studio with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public IEnumerable<PersonDTO> GetAllPersons()
        {
            _logger.LogInformation("Starting get all persons.");
            try
            {
                _logger.LogInformation("Call GetAllPersons.");
                IQueryable<Person> persons = _personRepository.GetAllPersons();

                _logger.LogInformation("Get all persons successfull.");

                return persons.Select(e => _mapper.PersonModelToDTO(e)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Get all persons has thrown an exception: {e.Message}.");

                return new List<PersonDTO>();
            }
        }
    }
}
