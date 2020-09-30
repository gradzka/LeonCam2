// UserService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Users
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Enums;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Models.DB;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using LeonCam2.Services.JwtTokens;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class UserService : IUserService
    {
        private static readonly string UserIsRegisteredInfo = "User is registered...";

        private readonly ILogger<UserService> logger;
        private readonly IStringLocalizer<UserService> localizer;
        private readonly Settings settings;
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenService jwtTokenService;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IOptions<Settings> settings,
            IStringLocalizer<UserService> localizer,
            IJwtTokenService jwtTokenService)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.settings = settings.Value;
            this.localizer = localizer;
            this.jwtTokenService = jwtTokenService;
        }

        public async Task<string> GetLeadingQuestionAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            string leadingQuestion = await this.userRepository.GetLeadingQuestionAsync(username).ConfigureAwait(false);

            if (string.IsNullOrEmpty(leadingQuestion))
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.LeadingQuestionEmpty)]);
            }

            return leadingQuestion;
        }

        public async Task<string> LoginAsync(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            var user = await this.userRepository.GetUserAsync(loginModel.Username).ConfigureAwait(false);

            if (user != null)
            {
                if (user.Password == $"{loginModel.Password}{loginModel.Username}{user.CreationDate}".GetSHA512Hash())
                {
                    if (user.LastLoginAttemptDate > DateTime.Now.AddMinutes(-this.settings.BlockTimeInMinutes) && user.AccessFailedCount >= this.settings.MaxNumberOfLoginAttempts)
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        await this.userRepository.UpdateAsync(user).ConfigureAwait(false);

                        throw new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]);
                    }
                    else
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        user.AccessFailedCount = 0;
                        await this.userRepository.UpdateAsync(user).ConfigureAwait(false);
                    }

                    return this.jwtTokenService.CreateToken(user.Id);
                }
                else
                {
                    user.LastLoginAttemptDate = DateTime.Now;
                    user.AccessFailedCount = Math.Min(this.settings.MaxNumberOfLoginAttempts, user.AccessFailedCount + 1);
                    await this.userRepository.UpdateAsync(user).ConfigureAwait(false);

                    throw new InternalException(this.localizer[nameof(UserServiceMessages.InproperLoginData)]);
                }
            }
            else
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.InproperLoginData)]);
            }
        }

        public void Logout(string token)
        {
            this.jwtTokenService.AddTokenToBlackList(token);
        }

        public async Task RegisterAsync(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                throw new ArgumentNullException(nameof(registerModel));
            }

            if (string.IsNullOrEmpty(registerModel.Username))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            if (registerModel.Password != registerModel.RepeatedPassword)
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.PasswordsMustBeTheSame)]);
            }

            if (string.IsNullOrEmpty(registerModel.Password))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.PasswordCannotBeEmpty)]);
            }

            this.logger.LogInformation(UserIsRegisteredInfo);
            DateTime dateTimeNow = DateTime.Now;

            if (await this.userRepository.GetUserAsync(registerModel.Username) != null)
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.UsernameAlreadyUsed)]);
            }

            string passwordData = $"{registerModel.Password}{registerModel.Username}{dateTimeNow}";

            User user = new User
            {
                Username = registerModel.Username,
                Password = passwordData.GetSHA512Hash(),
                LeadingQuestion = null,
                LastLoginAttemptDate = dateTimeNow,
                AccessFailedCount = 0,
                CreationDate = dateTimeNow,
                ModifiedDate = dateTimeNow,
            };

            this.logger.LogDebug($"Registered user data: {user}");

            await this.userRepository.InsertAsync(user).ConfigureAwait(false);
        }

        public async Task<string> CheckAnswerAsync(LeadingQuestionModel leadingQuestionModel)
        {
            if (leadingQuestionModel == null)
            {
                throw new ArgumentNullException(nameof(leadingQuestionModel));
            }

            if (string.IsNullOrEmpty(leadingQuestionModel.Username))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            if (string.IsNullOrEmpty(leadingQuestionModel.Answer))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.AnswerCannotBeEmpty)]);
            }

            var user = await this.userRepository.GetUserAsync(leadingQuestionModel.Username).ConfigureAwait(false);

            if (user != null)
            {
                if (user.LeadingQuestionAnswer == $"{leadingQuestionModel.Answer}{leadingQuestionModel.Username}{user.CreationDate}".GetSHA512Hash())
                {
                    if (user.LastLoginAttemptDate > DateTime.Now.AddMinutes(-this.settings.BlockTimeInMinutes) && user.AccessFailedCount >= this.settings.MaxNumberOfLoginAttempts)
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        await this.userRepository.UpdateAsync(user).ConfigureAwait(false);
                        throw new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]);
                    }
                    else
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        user.AccessFailedCount = 0;
                        await this.userRepository.UpdateAsync(user).ConfigureAwait(false);
                    }

                    return this.jwtTokenService.CreateToken(user.Id);
                }
                else
                {
                    user.LastLoginAttemptDate = DateTime.Now;
                    user.AccessFailedCount = Math.Min(this.settings.MaxNumberOfLoginAttempts, user.AccessFailedCount + 1);
                    await this.userRepository.UpdateAsync(user).ConfigureAwait(false);

                    throw new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]);
                }
            }
            else
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.InproperUsername)]);
            }
        }

        public Task ChangeUsernameAsync(int userId, ChangeUsernameModel changeUsernameModel)
        {
            throw new NotImplementedException();
        }

        public Task ChangePasswordAsync(int userId, ChangePasswordModel changePasswordModel)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccountAsync(int userId, string password)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAccountAsync(int userId, string password)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            var user = await this.userRepository.GetAsync(userId).ConfigureAwait(false);

            if (user == null)
            {
                return false;
            }

            return user.Password == $"{password}{user.Username}{user.CreationDate}".GetSHA512Hash();
        }
    }
}
