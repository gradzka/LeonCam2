// UsersController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Models.Users;
    using LeonCam2.Services.Users;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly string IsNullError = " is null";
        private static readonly string UserIsRegisteredInfo = "User is registered";
        private static readonly string UserRegistrationStartedInfo = "User registration started...";

        private readonly ILogger<UsersController> logger;
        private readonly IUserService userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            return this.Ok(await this.userService.Login(loginModel));
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

            await this.userService.Register(registerModel);

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

            await this.userService.ResetPassword(eMail);

            return this.Ok();
        }
    }
}