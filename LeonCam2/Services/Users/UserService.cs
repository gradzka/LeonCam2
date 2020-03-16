// UserService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Users
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using Microsoft.Extensions.Logging;

    public class UserService : IUserService
    {
        private static readonly string UserIsRegisteredInfo = "User is registered...";

        private readonly ILogger<UserService> logger;
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            throw new NotImplementedException();
        }

        public async Task Register(RegisterModel registerModel)
        {
            this.logger.LogInformation(UserIsRegisteredInfo);
            DateTime dateTimeNow = DateTime.Now;

            string passwordData = $"{registerModel.Password}{registerModel.Username}{dateTimeNow}";

            User user = new User
            {
                Username = registerModel.Username,
                Password = passwordData.GetSHA512Hash(),
                Email = null,
                LastLoginAttemptDate = dateTimeNow,
                LastLogoutDate = null,
                LoginAttemptCounter = 0,
                CreationDate = dateTimeNow,
                ModifiedDate = dateTimeNow,
            };

            this.logger.LogDebug($"Registered user data: {user}");

            await this.userRepository.InsertAsync(user);
        }

        public async Task ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(nameof(email));
            }

            throw new NotImplementedException();
        }
    }
}
