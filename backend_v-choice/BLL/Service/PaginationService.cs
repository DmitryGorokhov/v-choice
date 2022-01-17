﻿using System;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaginationService(ICommentsRepository cr, IFilmRepository fr, IFavoriteRepository favr, ILogger<PaginationService> logger, IMapper mapper)
        {
            _commentsRepository = cr;
            _filmRepository = fr;
            _favoriteRepository = favr;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationDTO<CommentDTO>> GetCommentsPagination(PaginationQuery query, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Starting get {query.OnPageCount} comments on {query.PageNumber} page.");
            try
            {
                _logger.LogInformation("Call GetCommentsByPageAsync.");

                var answer = await _commentsRepository.GetCommentsByPageAsync(
                    query.PageNumber,
                    query.OnPageCount,
                    query.FilmId,
                    query.CommonOrder ?? true,
                    query.MyCommentsFirst ?? false,
                    user
                    );
                
                _logger.LogInformation($"Get {query.OnPageCount} comments on {query.PageNumber} page successfully. Pack result into object before return.");
                var res = new PaginationDTO<CommentDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => _mapper.CommentModelToDTO(e)).ToList(),
                };

                return res;
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

                Pagination<Film> answer = await _favoriteRepository.GetFavoriteFilmsByPageAsync(
                    query.PageNumber,
                    query.OnPageCount,
                    query.CommonOrder ?? true,
                    user);
                
                _logger.LogInformation($"Get {query.OnPageCount} favorite films on {query.PageNumber} page successfully. Pack result into object before return.");
                var res = new PaginationDTO<FilmDTO>(query)
                {
                    TotalCount = answer.TotalCount,
                    Items = answer.Items.Select(e => _mapper.FilmModelToDTO(e)).ToList(),
                };

                return res;
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
                int genreId = query.GenreId != null ? (int)query.GenreId : -1;

                _logger.LogInformation("Call GetFilmsByPageAsync.");
                Pagination<Film> answer = await _filmRepository.GetFilmsByPageAsync(
                    query.PageNumber,
                    query.OnPageCount,
                    genreId,
                    query.SortBy ?? 0,
                    query.CommonOrder ?? true,
                    query.HasCommentsOnly ?? false,
                    query.WithoutMyRateOnly ?? false,
                    user
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
