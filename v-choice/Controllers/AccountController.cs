using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using v_choice.Models;
using v_choice.Interfaces;
using Microsoft.Extensions.Logging;

namespace v_choice.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _users;

        public AccountController(IUserRepository users, ILogger<AccountController> logger)
        {
            _users = users;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation("Register new user.");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Start register.");
                // Добавление нового пользователя
                var result = await _users.UserRegisterAsync(model);
                if (result.Succeeded)
                {
                    var msg = new
                    {
                        message = "Добавлен новый пользователь: " + model.Email
                    };
                    _logger.LogInformation($"Register: added user with email equal {model.Email}");
                    _logger.LogInformation("Finish register.");
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
            else
            {
                _logger.LogWarning("Register: model state is not valid.");
                var errorMsg = new
                {
                    message = "Неверные входные данные.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/Account/Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            _logger.LogInformation("Login user.");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Start login.");
                var result = await _users.UserLogInAsync(model);
                if (result.Succeeded)
                {
                    var msg = new
                    {
                        message = "Выполнен вход пользователем: " + model.Email
                    };
                    _logger.LogInformation($"Login: login user with email equal {model.Email}");
                    _logger.LogInformation("Finish login.");
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
            else
            {
                _logger.LogWarning("Login: model state is not valid.");
                var errorMsg = new
                {
                    message = "Вход не выполнен.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/logoff")]
        public async Task<IActionResult> LogOff()
        {
            await _users.UserSignOutAsync();
            var msg = new
            {
                message = "Выполнен выход."
            };
            _logger.LogInformation("Logout user");
            return Ok(msg);
        }

        [HttpPost]
        [Route("api/Account/isAuthenticated")]
        public async Task<IActionResult> LoginAuthenticatedOff()
        {
            _logger.LogInformation("Check authenticated user");
            User usr = await _users.GetCurrentUserAsync(HttpContext.User);
            var message = usr == null ? "guest" : usr.UserName;
            var msg = new
            {
                message
            };
            _logger.LogInformation($"Authenticated: {msg.message}");
            return Ok(msg);
        }
    }
}