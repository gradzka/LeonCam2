// UsersController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Services.Users;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly string CheckingUsernameStartedInfo = "Checking username started...";
        private static readonly string IsNullError = " is null";
        private static readonly string UsernameIsCheckedInfo = "Username is checked";
        private static readonly string UserIsRegisteredInfo = "User is registered";
        private static readonly string UserRegistrationStartedInfo = "User registration started...";

        private readonly ILogger<UsersController> logger;
        private readonly IUserService userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost("CheckUsername")]
        public async Task<IActionResult> CheckUsername([FromBody] string username)
        {
            this.logger.LogInformation(CheckingUsernameStartedInfo);
            this.logger.LogDebug($"Username: {username}");

            if (string.IsNullOrEmpty(username))
            {
                this.logger.LogError($"{nameof(username)}{IsNullError}");
                throw new ArgumentException(nameof(username));
            }

            bool result = await this.userService.CheckUsername(username).ConfigureAwait(false);

            this.logger.LogInformation(UsernameIsCheckedInfo);

            return this.Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            return this.Ok(new { token = await this.userService.Login(loginModel).ConfigureAwait(false) });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            this.logger.LogInformation(UserRegistrationStartedInfo);
            this.logger.LogDebug($"RegisterModel data: {registerModel}");

            if (registerModel == null)
            {
                this.logger.LogError($"{nameof(registerModel)}{IsNullError}");
                throw new ArgumentNullException(nameof(registerModel));
            }

            await this.userService.Register(registerModel).ConfigureAwait(false);

            this.logger.LogInformation(UserIsRegisteredInfo);

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string eMail)
        {
            if (string.IsNullOrEmpty(eMail))
            {
                throw new ArgumentException(nameof(eMail));
            }

            await this.userService.ResetPassword(eMail).ConfigureAwait(false);

            return this.Ok();
        }
    }
}