using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using Microsoft.AspNetCore.Identity;
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

                return Ok(new LoginDTO()
                {
                    Result = false,
                    Message = "Неверные входные данные.",
                    Error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                });
            }

            (LoginDTO answer, IdentityResult res) = await _autorizationService.RegisterUserAsync(reg);

            if (!answer.Result)
            {
                if (res != null)
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                answer.Error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));

                _logger.LogInformation("Register: user isn't added. Errors:");
                foreach (string e in answer.Error)
                    _logger.LogInformation($"Register: {e}");
            }

            _logger.LogInformation("Finish register.");

            return Ok(answer);
        }

        [HttpPost]
        [Route("api/Account/Login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery log)
        {
            _logger.LogInformation("Login user.");

            LoginDTO answer = new LoginDTO();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login: model state is not valid.");

                answer.Result = false;
                answer.Message = "Вход не выполнен.";
                answer.Error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));

                return Ok(answer);
            }

            answer = await _autorizationService.LogInUserAsync(log);

            if (!answer.Result)
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                answer.Error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));

                _logger.LogInformation("Login: user isn't login. Errors:");
                foreach (string e in answer.Error)
                    _logger.LogInformation($"Login: {e}");
            }

            _logger.LogInformation("Finish login.");

            return Ok(answer);
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
        public async Task<IActionResult> CheckAuthenticatedUser()
        {
            _logger.LogInformation("Check authenticated user email");

            AuthenticatedUserDTO user = await _autorizationService.GetAuthenticatedUserAsync(HttpContext.User);
            if (user == null) return StatusCode(500);

            return Ok(user);
        }
    }
}
