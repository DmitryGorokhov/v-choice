using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;
using System.Security.Claims;

namespace BLL.Service
{
    public class PaginationService : IPaginationService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IFavoriteRepository _favoriteRepository ;
        private readonly IAutorizationService _autorizationService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaginationService(ICommentsRepository cr, IFilmRepository fr, IFavoriteRepository favr, IAutorizationService aus, ILogger<PaginationService> logger, IMapper mapper)
        {
            _commentsRepository = cr;
            _filmRepository = fr;
            _favoriteRepository = favr;
            _autorizationService = aus;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQuery query, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} comments on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetCommentsByPageAsync.");

                Pagination<Comment> answer;
                query.CommonOrder ??= true;

                if (query.MyCommentsFirst ?? false)
                {
                    string userId = (await _autorizationService.GetCurrentUserAsync(user)).Id;
                    answer = query.CommonOrder switch
                    {
                        true => await _commentsRepository.GetByDateDescendingUserFirstAsync(query.PageNumber, query.OnPageCount, query.FilmId, userId),
                        false => await _commentsRepository.GetByDateUserFirstAsync(query.PageNumber, query.OnPageCount, query.FilmId, userId),
                    };
                }

                answer = query.CommonOrder switch
                {
                    true => await _commentsRepository.GetByDateDescendingOnlyAsync(query.PageNumber, query.OnPageCount, query.FilmId),
                    false => await _commentsRepository.GetByDateOnlyAsync(query.PageNumber, query.OnPageCount, query.FilmId),
                };

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

        public async Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQuery query, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} favorite films on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetFavoriteFilmsByPageAsync.");

                string userId = (await _autorizationService.GetCurrentUserAsync(user)).Id;
                
                Pagination<Film> answer = (query.CommonOrder ?? true) switch
                {
                    true => await _favoriteRepository.GetByDateDescendingAsync(query.PageNumber, query.OnPageCount, userId),
                    false => await _favoriteRepository.GetByDateAsync(query.PageNumber, query.OnPageCount, userId)
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

        public async Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQuery query, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} films on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetFilmsByPageAsync.");

                string userId = (await _autorizationService.GetCurrentUserAsync(user))?.Id;

                Pagination<Film> answer = await _filmRepository.GetFilmsByPageAsync(
                    query.PageNumber,
                    query.OnPageCount,
                    query.GenreId ?? 0,
                    query.SortBy ?? 0,
                    query.CommonOrder ?? true,
                    query.HasCommentsOnly ?? false,
                    query.WithoutMyRateOnly ?? false,
                    userId
                    );
                
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
