using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly IAutorizationService _autorizationService;
        private readonly ILogger _logger;

        public AccountController(IAutorizationService aus, ILogger<AccountController> logger)
        {
            _autorizationService = aus;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterQuery reg)
        {
            _logger.LogInformation("Register new user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Register: model state is not valid.");
                var errorMsg = new
                {
                    message = "Неверные входные данные.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                
                return Ok(errorMsg);
            }

            // Добавление нового пользователя
            var result = await _autorizationService.RegisterUserAsync(reg);
            if (result != null && result.Succeeded)
            {
                var msg = new
                {
                    message = "Добавлен новый пользователь: " + reg.Email
                };

                _logger.LogInformation($"Register: added user with email equal {reg.Email}. \nFinish register.");
                
                return Ok(msg);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var errorMsg = new
                {
                    message = "Пользователь не добавлен.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                _logger.LogInformation("Register: user isn't added. Errors:");
                foreach (string e in errorMsg.error)
                    _logger.LogInformation($"Register: {e}");

                _logger.LogInformation("Finish register.");

                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/Account/Login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery log)
        {
            _logger.LogInformation("Login user.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login: model state is not valid.");
                var errorMsg = new
                {
                    message = "Вход не выполнен.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                
                return Ok(errorMsg);
            }

            var result = await _autorizationService.LogInUserAsync(log);
            if (result != null && result.Succeeded)
            {
                var msg = new
                {
                    message = "Выполнен вход пользователем: " + log.Email
                };

                _logger.LogInformation($"Login: login user with email equal {log.Email}. \nFinish login.");
                
                return Ok(msg);
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                var errorMsg = new
                {
                    message = "Вход не выполнен.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                _logger.LogInformation("Login: user isn't login. Errors:");
                foreach (string e in errorMsg.error)
                    _logger.LogInformation($"Login: {e}");

                _logger.LogInformation("Finish login.");
                
                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/logoff")]
        public async Task<IActionResult> LogOff()
        {
            _logger.LogInformation("Start logout user");
            await _autorizationService.UserSignOutAsync();
            var msg = new
            {
                message = "Выполнен выход."
            };
            _logger.LogInformation("Finish logout user.");
            
            return Ok(msg);
        }

        [HttpPost]
        [Route("api/Account/isAuthenticated")]
        public async Task<IActionResult> LoginAuthenticatedOff()
        {
            _logger.LogInformation("Check authenticated user email");
            
            string message = await _autorizationService.GetCurrentUserEmailAsync(HttpContext.User);
            var msg = new
            {
                message
            };

            return Ok(msg);
        }
    }
}
