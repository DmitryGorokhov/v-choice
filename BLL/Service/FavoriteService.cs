using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;

namespace BLL.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapperDTOtoModel _mapper;

        public FavoriteService(IUserRepository ur, ILogger<FavoriteService> logger, IMapperDTOtoModel mapper)
        {
            _userRepository = ur;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddFavoriteFilmAsync(FilmDTO film, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start adding film with Id equal {film.Id} to favorites of authorized user.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);

                _logger.LogInformation("Call AddFavoriteFilmAsync.");
                await _userRepository.AddFavoriteFilmAsync(f, user);
                
                _logger.LogInformation($"Add film in favorites: film with id={film.Id} was added to favorite films.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Add film in favorite has thrown an exception: {e.Message}.");
            }
        }

        public async Task<bool?> CheckFilmIsAdded(int id, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start сhecking film with id={id} in favorites.");
            try
            {
                _logger.LogInformation("Call CheckFilmIsAdded.");
                var res = await _userRepository.CheckFilmIsAdded(id, user);
                
                string message = res == null
                    ? $"Check film in favorites: film with Id equal {id} not found."
                    : $"Check film in favorites with id={id}: Ok - {res}.";
                _logger.LogInformation(message);
                
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Check film in favorites with id={id} has thrown an exception: {e.Message}.");
                
                return null;
            }
        }

        public async Task RemoveFilmFromFavorite(FilmDTO film, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start deleting film with id={film.Id} from favorites of authorized user.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);

                _logger.LogInformation("Call RemoveFilmFromFavorite.");
                await _userRepository.RemoveFilmFromFavorite(f, user);
                
                _logger.LogInformation($"Delete film from favorites: film with id={film.Id} was deleted from favorite films.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film from favorites has thrown an exception: {e.Message}.");
            }
        }
    }
}
