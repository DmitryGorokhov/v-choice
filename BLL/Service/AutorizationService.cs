using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AutorizationService : IAutorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public AutorizationService(IUserRepository ur, ILogger<AutorizationService> logger)
        {
            _userRepository = ur;
            _logger = logger;
        }

        public async Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            _logger.LogInformation("Start login user.");
            try
            {
                _logger.LogInformation("Call UserLogInAsync.");
                User usr = await _userRepository.GetCurrentUserAsync(user);
                _logger.LogInformation("Convert to DTO.");
                UserDTO u = new UserDTO(usr);
                _logger.LogInformation($"Authenticated: {u.Email}");
                
                return u;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCurrentUserAsync has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<SignInResult> LogInUserAsync(LoginDTO log)
        {
            _logger.LogInformation("Start login user.");
            try
            {
                _logger.LogInformation("Convert to model.");
                LoginModel model = new LoginModel()
                {
                    Email = log.Email,
                    Password = log.Password,
                    RememberMe = log.RememberMe
                };

                _logger.LogInformation("Call UserLogInAsync.");
                var result = await _userRepository.UserLogInAsync(model);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"UserLogInAsync has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO reg)
        {
            _logger.LogInformation("Start register.");
            try
            {
                _logger.LogInformation("Convert to model.");
                RegisterModel model = new RegisterModel() 
                {
                    Email = reg.Email,
                    Password = reg.Password,
                    PasswordConfirm = reg.PasswordConfirm
                };

                _logger.LogInformation("Call UserRegisterAsync.");
                var result = await _userRepository.UserRegisterAsync(model);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"UserRegisterAsync has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UserSignOutAsync()
        {
            _logger.LogInformation("Call UserSignOutAsync.");
            await _userRepository.UserSignOutAsync();
        }
    }
}
