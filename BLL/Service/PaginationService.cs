using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class PaginationService : IPaginationService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public PaginationService(ICommentsRepository cr, IFilmRepository fr, IUserRepository ur, ILogger<PaginationService> logger)
        {
            _commentsRepository = cr;
            _filmRepository = fr;
            _userRepository = ur;
            _logger = logger;
        }

        public async Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQuery query)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} comments on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetCommentsByPageAsync.");
                var answer = await _commentsRepository.GetCommentsByPageAsync(query.PageNumber, query.OnPageCount);
                _logger.LogInformation($"Get {query.OnPageCount} comments on {query.PageNumber} page successfully.");
                
                _logger.LogInformation($"Pack result object.");
                var res = new PaginationDTO<CommentDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => new CommentDTO(e)).ToList(),
                };

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get {query.OnPageCount} comments on {query.PageNumber} page has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<PaginationDTO<FilmDTO>> GetFavoriteFilmsPagination(PaginationQuery query)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} favorite films on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetFavoriteFilmsByPageAsync.");
                Pagination<Film> answer = await _userRepository.GetFavoriteFilmsByPageAsync(query.PageNumber, query.OnPageCount);
                _logger.LogInformation($"Get {query.OnPageCount} favorite films on {query.PageNumber} page successfully.");

                _logger.LogInformation($"Pack result object.");
                var res = new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => new FilmDTO(e)).ToList(),
                };

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get {query.OnPageCount} favorite films on {query.PageNumber} page has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<PaginationDTO<FilmDTO>> GetFilmsPagination(PaginationQuery query)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} films on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetFilmsByPageAsync.");
                int genreId = query.GenreId != null ? (int)query.GenreId : -1;

                Pagination<Film> answer = await _filmRepository.GetFilmsByPageAsync(query.PageNumber, query.OnPageCount, genreId);
                _logger.LogInformation($"Get {query.OnPageCount} films on {query.PageNumber} page successfully.");

                _logger.LogInformation($"Pack result object.");
                var res = new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => new FilmDTO(e)).ToList(),
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
