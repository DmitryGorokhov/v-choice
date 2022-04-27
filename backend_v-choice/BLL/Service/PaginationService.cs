using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Enum;
using DAL.Interface;
using DAL.Model;
using Microsoft.Extensions.Logging;
using System;
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
                _logger.LogInformation("Call GetCommentsByPageAsync.");

                Pagination<Comment> answer;

                if (query.MyCommentsFirst)
                {
                    string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;
                    answer = query.CommonOrder switch
                    {
                        true => await _paginationRepository.GetCommentsByDateDescendingUserFirstAsync(query.PageNumber, query.OnPageCount, query.FilmId, userId),
                        false => await _paginationRepository.GetCommentsByDateUserFirstAsync(query.PageNumber, query.OnPageCount, query.FilmId, userId),
                    };
                }
                else
                {
                    answer = query.CommonOrder switch
                    {
                        true => await _paginationRepository.GetCommentsByDateDescendingOnlyAsync(query.PageNumber, query.OnPageCount, query.FilmId),
                        false => await _paginationRepository.GetCommentsByDateOnlyAsync(query.PageNumber, query.OnPageCount, query.FilmId),
                    };
                }

                _logger.LogInformation($"Get {query.OnPageCount} comments on {query.PageNumber} page successfully. Pack result into object before return.");

                return new PaginationDTO<CommentDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => _mapper.CommentModelToDTO(e)).ToList(),
                };
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
                _logger.LogInformation("Call GetFavoriteFilmsByPageAsync.");

                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;

                Pagination<Film> answer = query.CommonOrder switch
                {
                    true => await _paginationRepository.GetFavoritesByDateDescendingAsync(query.PageNumber, query.OnPageCount, userId),
                    false => await _paginationRepository.GetFavoritesByDateAsync(query.PageNumber, query.OnPageCount, userId)
                };

                _logger.LogInformation($"Get {query.OnPageCount} favorite films on {query.PageNumber} page successfully. Pack result into object before return.");

                return new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => _mapper.FilmModelToDTO(e)).ToList(),
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

                if (query.SortBy != null)
                {
                    query.SortBy = SortingType.NotSet;
                }

                if (query.GenreId != null && query.GenreId > 0)
                {
                    _logger.LogInformation($"Write genre with Id={query.GenreId} was requested by catalog filter. Call GenreRequestedCounter.");
                    await _genreRepository.GenreRequestedCounter((int)query.GenreId);
                }

                Pagination<Film> answer = query.SortBy switch
                {
                    SortingType.NotSet => answer = await _paginationRepository.GetFilmsAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    SortingType.Created => await _paginationRepository.GetFilmsSortedByCreatedAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    SortingType.CreatedDesc => await _paginationRepository.GetFilmsSortedByCreatedDescAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    SortingType.Year => await _paginationRepository.GetFilmsSortedByYearAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    SortingType.YearDesc => await _paginationRepository.GetFilmsSortedByYearDescAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    SortingType.Rate => await _paginationRepository.GetFilmsSortedByRateAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    SortingType.RateDesc => await _paginationRepository.GetFilmsSortedByRateDescAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false),
                    _ => answer = await _paginationRepository.GetFilmsAsync(
                        query.PageNumber,
                        query.OnPageCount,
                        query.GenreId ?? 0,
                        query.HasCommentsOnly ?? false,
                        query.HasRateOnly ?? false)
                };

                _logger.LogInformation($"Get {query.OnPageCount} films on {query.PageNumber} page successfully. Pack result into object before return.");
                var res = new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => _mapper.FilmModelToDTO(e)).ToList(),
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
