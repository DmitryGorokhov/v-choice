using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;

namespace BLL.Service
{
    public class AutorizationService : IAutorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AutorizationService(IUserRepository ur, ILogger<AutorizationService> logger, IMapper mapper)
        {
            _userRepository = ur;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<string> GetCurrentUserNameAsync(ClaimsPrincipal user)
        {
            _logger.LogInformation("Starting get name of current user.");
            try
            {
                _logger.LogInformation("Call GetCurrentUserModelAsync.");
                User u = await GetCurrentUserModelAsync(user);

                if (u == null)
                {
                    _logger.LogInformation($"Current user: guest");

                    return "guest";
                }
            
                _logger.LogInformation($"Authenticated: {u.Email}");

                return u.Email;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get current user name has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<User> GetCurrentUserModelAsync(ClaimsPrincipal user)
        {
            _logger.LogInformation("Get current user model.");
            
            return await _userRepository.GetCurrentUserAsync(user);
        }

        public async Task<SignInResult> LogInUserAsync(LoginQuery log)
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

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Login user has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterQuery reg)
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

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Register has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UserSignOutAsync()
        {
            _logger.LogInformation("Call UserSignOutAsync.");
            await _userRepository.UserSignOutAsync();
        }

        public async Task<bool> CheckIfCurrentUserIsAdmin(ClaimsPrincipal user)
        {
            _logger.LogInformation("Starting check if current user is admin.");
            try
            {
                _logger.LogInformation("Call GetCurrentUserModelAsync.");
                User u = await GetCurrentUserModelAsync(user);

                if (u == null)
                {
                    _logger.LogInformation($"Current user: guest, not admin");

                    return false;
                }

                bool res = await _userRepository.CheckUserHasAdminRoleAsync(u);

                _logger.LogInformation($"Authenticated: {u.Email} as {(res ? "an admin" : "an user")}");

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get current user info has thrown an exception: {e.Message}.");

                return false;
            }
        }
    }
}
