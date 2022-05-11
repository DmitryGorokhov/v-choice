using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Enum;
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
    public class PaginationService : IPaginationService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IPaginationRepository _paginationRepository;
        private readonly IAutorizationService _autorizationService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaginationService(IPaginationRepository pr, IGenreRepository gr, IAutorizationService aus, ILogger<PaginationService> logger, IMapper mapper)
        {
            _paginationRepository = pr;
            _genreRepository = gr;
            _autorizationService = aus;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQueryComments query, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} comments on {query.PageNumber} page.");
            try
            {
                if (query.MyCommentsFirst)
                {
                    _logger.LogInformation("Call GetCurrentUserModelAsync.");
                    string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;

                    _logger.LogInformation($"Call {(query.CommonOrder ? "GetCommentsByDateDescendingUserFirst" : "GetCommentsByDateUserFirst")}.");
                    IEnumerable<Comment> seq = query.CommonOrder switch
                    {
                        true => await _paginationRepository.GetCommentsByDateDescendingUserFirst(userId, query.FilmId),
                        false => await _paginationRepository.GetCommentsByDateUserFirst(userId, query.FilmId),
                    };

                    int total = seq.Count();
                    var items = seq.Skip((query.PageNumber - 1) * query.OnPageCount).Take(query.OnPageCount).Select(e => _mapper.CommentModelToDTO(e));
                    _logger.LogInformation($"Get {query.OnPageCount} comments on {query.PageNumber} page successfully. Pack result into object before return.");
                    
                    return new PaginationDTO<CommentDTO>(query)
                    {
                        TotalCount = total,
                        Items = items,
                    };
                }
                else
                {
                    _logger.LogInformation($"Call {(query.CommonOrder ? "GetCommentsByDateDescendingOnly" : "GetCommentsByDateOnly")}.");
                    IQueryable<Comment> collection = query.CommonOrder switch
                    {
                        true => _paginationRepository.GetCommentsByDateDescendingOnly(query.FilmId),
                        false => _paginationRepository.GetCommentsByDateOnly(query.FilmId),
                    };

                    _logger.LogInformation($"Get {query.OnPageCount} comments on {query.PageNumber} page successfully. Pack result into object before return.");
                    (int total, var items) = await _paginationRepository.SplitByPagesAsync(collection, query.PageNumber, query.OnPageCount);

                    return new PaginationDTO<CommentDTO>(query)
                    {
                        TotalCount = total,
                        Items = items.Select(e => _mapper.CommentModelToDTO(e)).ToList(),
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Get {query.OnPageCount} comments on {query.PageNumber} page has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQueryFavorites query, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} favorite films on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetCurrentUserModelAsync.");
                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;

                _logger.LogInformation($"Call {(query.CommonOrder ? "GetFavoritesByDateDescending" : "GetFavoritesByDate")}.");
                IQueryable<Film> collection = query.CommonOrder switch
                {
                    true => _paginationRepository.GetFavoritesByDateDescending(userId),
                    false => _paginationRepository.GetFavoritesByDate(userId)
                };

                _logger.LogInformation($"Get {query.OnPageCount} favorite films on {query.PageNumber} page successfully. Pack result into object before return.");
                (int total, var items) = await _paginationRepository.SplitByPagesAsync(collection, query.PageNumber, query.OnPageCount);

                return new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = total,
                    Items = items.Select(e => _mapper.FilmModelToDTO(e)).ToList(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Get {query.OnPageCount} favorite films on {query.PageNumber} page has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQueryFilms query)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} films on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetFilmsByPageAsync.");

                if (query.GenreId != null && query.GenreId > 0)
                {
                    _logger.LogInformation($"Write genre with Id={query.GenreId} was requested by catalog filter. Call GenreRequestedCounter.");
                    await _genreRepository.GenreRequestedCounter((int)query.GenreId);
                }

                _logger.LogInformation("Call GetAllFilms.");
                IQueryable<Film> collection = _paginationRepository.GetAllFilms();

                if ((query.GenreId ?? 0) != 0)
                {
                    _logger.LogInformation("Call GetFilmsByGenreId.");
                    collection = _paginationRepository.GetFilmsByGenreId(collection, (int)query.GenreId);
                }

                if (query.HasCommentsOnly ?? false)
                {
                    _logger.LogInformation("Call GetFilmsWithCommentsOnly.");
                    collection = _paginationRepository.GetFilmsWithCommentsOnly(collection);
                }

                if (query.HasRateOnly ?? false)
                {
                    _logger.LogInformation("Call GetFilmsWithRateOnly.");
                    collection = _paginationRepository.GetFilmsWithRateOnly(collection);
                }

                _logger.LogInformation($"Film pagination: sort collection by {query.SortBy}.");
                collection = (query.SortBy ?? SortingType.NotSet) switch
                {
                    SortingType.NotSet => collection,
                    SortingType.Created => _paginationRepository.GetFilmsByCreated(collection),
                    SortingType.CreatedDesc => _paginationRepository.GetFilmsByCreatedDesc(collection),
                    SortingType.Year => _paginationRepository.GetFilmsByYear(collection),
                    SortingType.YearDesc => _paginationRepository.GetFilmsByYearDesc(collection),
                    SortingType.Rate => _paginationRepository.GetFilmsByRate(collection),
                    SortingType.RateDesc => _paginationRepository.GetFilmsByDesc(collection),
                    _ => collection
                };

                _logger.LogInformation("Call SplitByPagesAsync.");
                (int total, var items) = await _paginationRepository.SplitByPagesAsync(collection, query.PageNumber, query.OnPageCount);

                _logger.LogInformation($"Get {query.OnPageCount} films on {query.PageNumber} page successfully. Pack result into object before return.");
                var res = new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = total,
                    Items = items.Select(e => _mapper.FilmModelToDTO(e)).ToList(),
                };

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get {query.OnPageCount} films on {query.PageNumber} page has thrown an exception: {e.Message}.");

                return null;
            }
        }
    }
}
