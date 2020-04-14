// UserService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Users
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using LeonCam2.Enums;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class UserService : IUserService
    {
        private static readonly string UserIsRegisteredInfo = "User is registered...";

        private readonly ILogger<UserService> logger;
        private readonly IStringLocalizer<UserService> localizer;
        private readonly Settings settings;
        private readonly IUserRepository userRepository;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IOptions<Settings> settings,
            IStringLocalizer<UserService> localizer)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.settings = settings.Value;
            this.localizer = localizer;
        }

        public async Task<string> GetLeadingQuestion(string username)
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

        public async Task<string> Login(LoginModel loginModel)
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
                        await this.userRepository.UpdateAsync(user);

                        throw new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]);
                    }
                    else
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        user.AccessFailedCount = 0;
                        await this.userRepository.UpdateAsync(user);
                    }

                    return this.CreateJwtToken(user.Id);
                }
                else
                {
                    user.LastLoginAttemptDate = DateTime.Now;
                    user.AccessFailedCount = Math.Min(this.settings.MaxNumberOfLoginAttempts, user.AccessFailedCount + 1);
                    await this.userRepository.UpdateAsync(user);

                    throw new InternalException(this.localizer[nameof(UserServiceMessages.InproperLoginData)]);
                }
            }
            else
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.InproperLoginData)]);
            }
        }

        public async Task Register(RegisterModel registerModel)
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

            if (this.userRepository.GetUserAsync(registerModel.Username).Result != null)
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
                LastLogoutDate = null,
                AccessFailedCount = 0,
                CreationDate = dateTimeNow,
                ModifiedDate = dateTimeNow,
            };

            this.logger.LogDebug($"Registered user data: {user}");

            await this.userRepository.InsertAsync(user).ConfigureAwait(false);
        }

        public async Task<string> CheckAnswer(string username, string answer)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]);
            }

            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentException(this.localizer[nameof(UserServiceMessages.AnswerCannotBeEmpty)]);
            }

            var user = await this.userRepository.GetUserAsync(username).ConfigureAwait(false);

            if (user != null)
            {
                if (user.LeadingQuestionAnswer == $"{answer}{username}{user.CreationDate}".GetSHA512Hash())
                {
                    if (user.LastLoginAttemptDate > DateTime.Now.AddMinutes(-this.settings.BlockTimeInMinutes) && user.AccessFailedCount >= this.settings.MaxNumberOfLoginAttempts)
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        await this.userRepository.UpdateAsync(user);
                        throw new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]);
                    }
                    else
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        user.AccessFailedCount = 0;
                        await this.userRepository.UpdateAsync(user);
                    }

                    return this.CreateJwtToken(user.Id);
                }
                else
                {
                    user.LastLoginAttemptDate = DateTime.Now;
                    user.AccessFailedCount = Math.Min(this.settings.MaxNumberOfLoginAttempts, user.AccessFailedCount + 1);
                    await this.userRepository.UpdateAsync(user);

                    throw new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]);
                }
            }
            else
            {
                throw new InternalException(this.localizer[nameof(UserServiceMessages.InproperUsername)]);
            }
        }

        private string CreateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.settings.JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
