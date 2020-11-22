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
            if (username.IsNullOrEmpty())
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            string leadingQuestion = await this.userRepository.GetLeadingQuestionAsync(username).ConfigureAwait(false);

            if (leadingQuestion.IsNullOrEmpty())
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
                if (user.CheckPassword(loginModel.Password))
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

            if (registerModel.Username.IsNullOrEmpty())
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            if (registerModel.Password != registerModel.RepeatedPassword)
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.PasswordsMustBeTheSame)]);
            }

            if (registerModel.Password.IsNullOrEmpty())
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

            if (leadingQuestionModel.Username.IsNullOrEmpty())
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            if (leadingQuestionModel.Answer.IsNullOrEmpty())
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

        public async Task ChangeUsernameAsync(int userId, ChangeUsernameModel changeUsernameModel)
        {
            if (changeUsernameModel == null)
            {
                throw new ArgumentNullException(nameof(changeUsernameModel));
            }

            this.logger.LogInformation($"ChangeUsername id:{userId} username:{changeUsernameModel.NewUsername}");

            if (changeUsernameModel.NewUsername.IsNullOrEmpty())
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            User user = await this.GetUser(userId);

            this.CheckUserPassword(user, changeUsernameModel.Password);

            if (changeUsernameModel.NewUsername == user.Username)
            {
                return;
            }

            if (await this.userRepository.GetUserAsync(changeUsernameModel.NewUsername) != null)
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.UsernameAlreadyUsed)]);
            }

            string passwordData = $"{changeUsernameModel.Password}{changeUsernameModel.NewUsername}{user.CreationDate}";

            user.Username = changeUsernameModel.NewUsername;
            user.Password = passwordData.GetSHA512Hash();
            user.ModifiedDate = DateTime.Now;

            await this.userRepository.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordModel changePasswordModel)
        {
            this.logger.LogInformation($"ChangePassword id:{userId}");

            if (changePasswordModel == null)
            {
                throw new ArgumentNullException(nameof(changePasswordModel));
            }

            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmNewPassword)
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.PasswordsMustBeTheSame)]);
            }

            User user = await this.GetUser(userId);

            this.CheckUserPassword(user, changePasswordModel.OldPassword);

            string passwordData = $"{changePasswordModel.NewPassword}{user.Username}{user.CreationDate}";

            if (passwordData == user.Password)
            {
                return;
            }

            user.Password = passwordData.GetSHA512Hash();
            user.ModifiedDate = DateTime.Now;

            await this.userRepository.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task ResetAccountAsync(int userId, string password)
        {
            this.logger.LogInformation($"ResetAccount id:{userId}");

            User user = await this.GetUser(userId);

            this.CheckUserPassword(user, password);
        }

        public async Task DeleteAccountAsync(int userId, string password)
        {
            this.logger.LogInformation($"DeleteAccount id:{userId}");

            User user = await this.GetUser(userId);

            this.CheckUserPassword(user, password);

            await this.userRepository.DeleteRowAsync(userId).ConfigureAwait(false);
        }

        private void CheckUserPassword(User user, string password)
        {
            if (!user.CheckPassword(password))
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.WrongPassword)]);
            }
        }

        private async Task<User> GetUser(int userId)
        {
            User user = await this.userRepository.GetAsync(userId).ConfigureAwait(false);

            if (user == null)
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.UserNotFound)]);
            }

            return user;
        }
    }
}
