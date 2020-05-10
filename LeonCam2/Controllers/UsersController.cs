﻿// UsersController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Enums;
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

            string leadingQuestion = await this.userService.GetLeadingQuestion(username).ConfigureAwait(false);

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

            return this.Ok(new { token = await this.userService.Login(loginModel).ConfigureAwait(false) });
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

            await this.userService.Register(registerModel).ConfigureAwait(false);

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

            return this.Ok(new { token = await this.userService.CheckAnswer(leadingQuestionModel).ConfigureAwait(false) });
        }
    }
}