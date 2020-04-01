// UserService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Users
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class UserService : IUserService
    {
        private static readonly string UserIsRegisteredInfo = "User is registered...";

        private readonly ILogger<UserService> logger;
        private readonly Settings settings;
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IOptions<Settings> settings)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.settings = settings.Value;
        }

        public async Task<string> GetLeadingQuestion(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty");
            }

            string leadingQuestion = await this.userRepository.GetLeadingQuestionAsync(username).ConfigureAwait(false);

            if (string.IsNullOrEmpty(leadingQuestion))
            {
                throw new ArgumentException("Leading question is empty");
            }

            return leadingQuestion;
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            var user = await this.userRepository.GetByUsernameAsync(loginModel.Username).ConfigureAwait(false);

            if (user != null)
            {
                if (user.Password == $"{loginModel.Password}{loginModel.Username}{user.ModifiedDate}".GetSHA512Hash())
                {
                    if (user.LastLoginAttemptDate > DateTime.Now.AddMinutes(-this.settings.BlockTimeInMinutes) && user.AccessFailedCount >= this.settings.MaxNumberOfLoginAttempts)
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        await this.userRepository.UpdateAsync(user);

                        throw new InternalException("Your account is locked - try again later");
                    }
                    else
                    {
                        user.LastLoginAttemptDate = DateTime.Now;
                        user.AccessFailedCount = 0;
                        await this.userRepository.UpdateAsync(user);
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(this.settings.JwtKey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    };

                    return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
                }
                else
                {
                    user.LastLoginAttemptDate = DateTime.Now;
                    user.AccessFailedCount = Math.Min(this.settings.MaxNumberOfLoginAttempts, user.AccessFailedCount + 1);
                    await this.userRepository.UpdateAsync(user);

                    throw new InternalException("Inproper login data");
                }
            }
            else
            {
                throw new InternalException("Inproper login data");
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
                throw new ArgumentException("Username cannot be empty");
            }

            if (registerModel.Password != registerModel.RepeatedPassword)
            {
                throw new ArgumentException("Passwords must be the same");
            }

            if (string.IsNullOrEmpty(registerModel.Password))
            {
                throw new ArgumentException("Password cannot be empty");
            }

            this.logger.LogInformation(UserIsRegisteredInfo);
            DateTime dateTimeNow = DateTime.Now;

            if (this.userRepository.GetByUsernameAsync(registerModel.Username).Result != null)
            {
                throw new InternalException("Username is already used");
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
                throw new ArgumentException(nameof(username));
            }

            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentException(nameof(answer));
            }

            //TODO: check answer and if success implement login -> return token

            throw new NotImplementedException();
        }
    }
}
