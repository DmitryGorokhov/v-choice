using BLL.Interface;
using DAL.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IAutorizationService _autorizationService;
        private readonly ILogger _logger;

        public FavoriteService(IFavoriteRepository favr, IAutorizationService aus, ILogger<FavoriteService> logger)
        {
            _favoriteRepository = favr;
            _autorizationService = aus;
            _logger = logger;
        }

        public async Task AddFavoriteFilmAsync(int filmId, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start adding film with Id equal {filmId} to favorites of authorized user.");
            try
            {
                _logger.LogInformation("Call AddFavoriteFilmAsync.");
                
                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;
                await _favoriteRepository.AddFavoriteFilmAsync(filmId, userId);
                
                _logger.LogInformation($"Add film in favorites: film with id={filmId} was added to favorite films.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Add film in favorite has thrown an exception: {e.Message}.");
            }
        }

        public async Task<bool?> CheckFilmIsAdded(int filmId, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start сhecking film with id={filmId} in favorites.");
            try
            {
                _logger.LogInformation("Call CheckFilmIsAdded.");
                
                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;
                var res = await _favoriteRepository.CheckFilmIsAdded(filmId, userId);
                
                string message = res == null
                    ? $"Check film in favorites: film with Id equal {filmId} not found."
                    : $"Check film in favorites with id={filmId}: Ok - {res}.";
                _logger.LogInformation(message);
                
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Check film in favorites with id={filmId} has thrown an exception: {e.Message}.");
                
                return null;
            }
        }

        public async Task RemoveFilmFromFavorite(int filmId, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start deleting film with id={filmId} from favorites of authorized user.");
            try
            {
                _logger.LogInformation("Call RemoveFilmFromFavorite.");
                
                string userId = (await _autorizationService.GetCurrentUserModelAsync(user)).Id;
                await _favoriteRepository.RemoveFilmFromFavorite(filmId, userId);
                
                _logger.LogInformation($"Delete film from favorites: film with id={filmId} was deleted from favorite films.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film from favorites has thrown an exception: {e.Message}.");
            }
        }
    }
}
