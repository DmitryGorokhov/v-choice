using BLL.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteService _favoriteRepository;
        private readonly ILogger _logger;

        public FavoriteService(IFavoriteService favr, ILogger<FavoriteService> logger)
        {
            _favoriteRepository = favr;
            _logger = logger;
        }

        public async Task AddFavoriteFilmAsync(int filmId, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start adding film with Id equal {filmId} to favorites of authorized user.");
            try
            {
                _logger.LogInformation("Call AddFavoriteFilmAsync.");
                await _favoriteRepository.AddFavoriteFilmAsync(filmId, user);
                
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
                var res = await _favoriteRepository.CheckFilmIsAdded(filmId, user);
                
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
                await _favoriteRepository.RemoveFilmFromFavorite(filmId, user);
                
                _logger.LogInformation($"Delete film from favorites: film with id={filmId} was deleted from favorite films.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film from favorites has thrown an exception: {e.Message}.");
            }
        }
    }
}
