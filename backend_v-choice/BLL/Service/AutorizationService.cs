using BLL.DTO;
using BLL.Interface;
using BLL.Query;
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

        public async Task<User> GetCurrentUserModelAsync(ClaimsPrincipal user)
        {
            _logger.LogInformation("Get current user model.");

            return await _userRepository.GetCurrentUserAsync(user);
        }

        public async Task<LoginDTO> LogInUserAsync(LoginQuery log)
        {
            _logger.LogInformation("Starting login user.");
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

                LoginDTO answer = new LoginDTO();

                if (result != null && result.Succeeded)
                {
                    answer.Result = true;
                    answer.Message = "Выполнен вход пользователем: " + log.Email;

                    _logger.LogInformation("Call GetUserByEmailAsync");
                    User u = await _userRepository.GetUserByEmailAsync(log.Email);
                    _logger.LogInformation("Call CheckUserHasAdminRoleAsync");

                    answer.User = new AuthenticatedUserDTO()
                    {
                        UserName = log.Email,
                        IsAdmin = await _userRepository.CheckUserHasAdminRoleAsync(u)
                    };

                    _logger.LogInformation($"Login: login user with email equal {log.Email}.");
                }
                else
                {
                    answer.Result = false;
                    answer.Message = "Вход не выполнен.";
                }

                return answer;
            }
            catch (Exception e)
            {
                _logger.LogError($"Login user has thrown an exception: {e.Message}.");

                return new LoginDTO() { Result = false, Message = "Вход не выполнен." };
            }
        }

        public async Task<(LoginDTO, IdentityResult)> RegisterUserAsync(RegisterQuery reg)
        {
            _logger.LogInformation("Starting register.");
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

                LoginDTO answer = new LoginDTO();

                if (result != null && result.Succeeded)
                {
                    answer.Result = true;
                    answer.Message = "Добавлен новый пользователь: " + reg.Email;
                    answer.User = new AuthenticatedUserDTO() { UserName = reg.Email, IsAdmin = false };

                    _logger.LogInformation($"Register: added user with email equal {reg.Email}.");
                }
                else
                {
                    answer.Result = false;
                    answer.Message = "Пользователь не добавлен.";
                }

                return (answer, result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Register has thrown an exception: {e.Message}.");

                return (new LoginDTO() { Result = false, Message = "Пользователь не добавлен." }, null);
            }
        }

        public async Task UserSignOutAsync()
        {
            _logger.LogInformation("Call UserSignOutAsync.");
            await _userRepository.UserSignOutAsync();
        }

        public async Task<AuthenticatedUserDTO> GetAuthenticatedUserAsync(ClaimsPrincipal user)
        {
            _logger.LogInformation("Starting get authenticated user.");
            try
            {
                User u = await GetCurrentUserModelAsync(user);

                if (u == null)
                {
                    _logger.LogInformation("Current user: guest, is not admin");

                    return new AuthenticatedUserDTO() { UserName = "guest", IsAdmin = false };
                }

                bool res = await _userRepository.CheckUserHasAdminRoleAsync(u);

                _logger.LogInformation($"Authenticated: {u.Email} as {(res ? "an admin" : "an user")}");

                return new AuthenticatedUserDTO() { UserName = u.Email, IsAdmin = res };
            }
            catch (Exception e)
            {
                _logger.LogError($"Get authenticated user has thrown an exception: {e.Message}.");

                return null;
            }
        }
    }
}
