// UsersController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using LeonCam2.Enums;
    using LeonCam2.Filters.AuthorizationFilters;
    using LeonCam2.Models.Users;
    using LeonCam2.Services.Users;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly string IsNullError = " is null";

        private readonly ILogger<UsersController> logger;
        private readonly IStringLocalizer<UsersController> localizer;
        private readonly IUserService userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IStringLocalizer<UsersController> localizer)
        {
            this.userService = userService;
            this.logger = logger;
            this.localizer = localizer;
        }

        [HttpPost("GetLeadingQuestion")]
        public async Task<IActionResult> GetLeadingQuestion([FromBody] string username)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.GettingLeadingQuestionStarted)]);
            this.logger.LogDebug($"Username: {username}");

            if (string.IsNullOrEmpty(username))
            {
                this.logger.LogError($"{nameof(username)}{IsNullError}");
                throw new ArgumentException(nameof(username));
            }

            string leadingQuestion = await this.userService.GetLeadingQuestionAsync(username).ConfigureAwait(false);

            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.GotLeadingQuestion)]);

            return this.Ok(new { leadingQuestion });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            return this.Ok(new { token = await this.userService.LoginAsync(loginModel).ConfigureAwait(false) });
        }

        [JwtTokenFilter]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            string token = this.ControllerContext.HttpContext.Request.Headers["Authorization"].First();
            this.userService.Logout(AuthenticationHeaderValue.Parse(token).Parameter);
            return this.Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.UserRegistrationStarted)]);
            this.logger.LogDebug($"RegisterModel data: {registerModel}");

            if (registerModel == null)
            {
                this.logger.LogError($"{nameof(registerModel)}{IsNullError}");
                throw new ArgumentNullException(nameof(registerModel));
            }

            await this.userService.RegisterAsync(registerModel).ConfigureAwait(false);

            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.UserRegistered)]);

            return this.Ok();
        }

        [HttpPost("CheckAnswer")]
        public async Task<IActionResult> CheckAnswer(LeadingQuestionModel leadingQuestionModel)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.CheckingAnswerStarted)]);
            this.logger.LogDebug($"Username: {leadingQuestionModel.Username}, Answer: {leadingQuestionModel.Answer}");

            if (leadingQuestionModel == null)
            {
                this.logger.LogError($"{nameof(leadingQuestionModel)}{IsNullError}");
                throw new ArgumentNullException(nameof(leadingQuestionModel));
            }

            return this.Ok(new { token = await this.userService.CheckAnswerAsync(leadingQuestionModel).ConfigureAwait(false) });
        }

        [HttpPost("ChangeUsername")]
        public async Task<IActionResult> ChangeUsername(string newUsername, string password)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.ChangingPasswordStarted)]);
            this.logger.LogDebug($"New username: {newUsername}");

            this.Validate(newUsername, nameof(newUsername));
            this.Validate(password, nameof(password));

            string token = this.ControllerContext.HttpContext.Request.Headers["Authorization"].First();

            await this.userService.ChangeUsernameAsync(newUsername, password).ConfigureAwait(false);

            return this.Ok();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] string oldPassword, string newPassword, string confirmNewPassword)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.ChangingPasswordStarted)]);

            this.Validate(oldPassword, nameof(oldPassword));
            this.Validate(newPassword, nameof(newPassword));
            this.Validate(confirmNewPassword, nameof(confirmNewPassword));

            string token = this.ControllerContext.HttpContext.Request.Headers["Authorization"].First();
            var a = AuthenticationHeaderValue.Parse(token);

            await this.userService.ChangePasswordAsync(oldPassword, newPassword, confirmNewPassword).ConfigureAwait(false);

            return this.Ok();
        }

        [HttpPost("ResetAccount")]
        public async Task<IActionResult> ResetAccount([FromBody] string password)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.ResettingAccountStarted)]);

            this.Validate(password, nameof(password));

            string token = this.ControllerContext.HttpContext.Request.Headers["Authorization"].First();

            await this.userService.ResetAccountAsync(password).ConfigureAwait(false);

            return this.Ok();
        }

        [HttpPost("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] string password)
        {
            this.logger.LogInformation(this.localizer[nameof(UsersControllerMessages.DeletingAccountStarted)]);

            this.Validate(password, nameof(password));

            string token = this.ControllerContext.HttpContext.Request.Headers["Authorization"].First();

            await this.userService.DeleteAccountAsync(password).ConfigureAwait(false);

            return this.Ok();
        }

        private void Validate(string param, string paramName)
        {
            if (string.IsNullOrEmpty(param))
            {
                this.logger.LogError($"{paramName}{IsNullError}");
                throw new ArgumentException(paramName);
            }
        }
    }
}