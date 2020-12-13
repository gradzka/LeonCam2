// UserServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Models.DB;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using LeonCam2.Services.JwtTokens;
    using LeonCam2.Services.Security;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class UserServiceTests
    {
        private static readonly string TestUser = "test";

        private readonly IOptions<Settings> options;
        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTests()
        {
            const string TestDate = "2020-03-29 15:00:42.9685001";
            const string TestPassword = "51A670987F4C067329A37CD8C8A493C38F49A0FE8BA1EE67FCB830EA681332F69FC7DB87AA4627E3A2735798F5DCC209A1D90CCA0C95DECA685F3CC7E6AD72E6";

            this.User = new User()
            {
                Username = TestUser,
                Password = TestPassword,
                LeadingQuestion = "Question",
                LeadingQuestionAnswer = TestPassword,
                LastLoginAttemptDate = DateTime.Parse(TestDate),
                AccessFailedCount = 0,
                Id = 1,
                CreationDate = DateTime.Parse(TestDate),
                ModifiedDate = DateTime.Parse(TestDate),
            };

            Settings settings = new Settings() { JwtKey = "SecretSecret key", MaxNumberOfLoginAttempts = 1 };
            var options = new Mock<IOptions<Settings>>();
            options.Setup(x => x.Value).Returns(settings);
            this.options = options.Object;

            this.localizer = new StringLocalizer<UserService>(
                new ResourceManagerStringLocalizerFactory(
                    Options.Create(new LocalizationOptions { ResourcesPath = "Resources" }),
                    NullLoggerFactory.Instance));
        }

        public User User { get; set; }

        [Theory]
        [ClassData(typeof(UserServiceTestsLoginData))]
        public async void Login_Test(LoginModel loginModel, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetUserAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                this.options,
                this.localizer,
                new JwtTokenService(null, this.options),
                new CryptoService());

            if (loginModel == null)
            {
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await userService.LoginAsync(loginModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else if (!(bool)testsMethodResult.Result)
            {
                InternalException ex = await Assert.ThrowsAsync<InternalException>(async () => await userService.LoginAsync(loginModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else
            {
                string token = await userService.LoginAsync(loginModel).ConfigureAwait(false);
                Assert.True(!token.IsNullOrEmpty());
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsRegisterData))]
        public async void Register_Test(RegisterModel registerModel, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetUserAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.InsertAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                new Mock<IOptions<Settings>>().Object,
                this.localizer,
                new Mock<IJwtTokenService>().Object,
                new CryptoService());

            if (registerModel == null)
            {
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await userService.RegisterAsync(registerModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else if (registerModel.Username == TestUser)
            {
                InternalException ex = await Assert.ThrowsAsync<InternalException>(async () => await userService.RegisterAsync(registerModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else if (!(bool)testsMethodResult.Result)
            {
                ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(async () => await userService.RegisterAsync(registerModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else
            {
                await userService.RegisterAsync(registerModel).ConfigureAwait(false);
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsGetLeadingQuestionData))]
        public async void GetLeadingQuestion_Test(string username, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetLeadingQuestionAsync(TestUser)).Returns(Task.FromResult(this.User.LeadingQuestion));
            userRepository.Setup(x => x.GetLeadingQuestionAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<string>(default));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                this.options,
                this.localizer,
                new Mock<IJwtTokenService>().Object,
                new CryptoService());

            try
            {
                string question = await userService.GetLeadingQuestionAsync(username).ConfigureAwait(false);

                Assert.True(testsMethodResult.Exception == null);
                Assert.Equal(this.User.LeadingQuestion, question);
            }
            catch (Exception ex)
            {
                Assert.True(testsMethodResult.Exception != null);
                Assert.Equal(ex.GetType(), testsMethodResult.Exception.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsCheckAnswerData))]
        public async void CheckAnswer_Test(LeadingQuestionModel leadingQuestionModel, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetUserAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                this.options,
                this.localizer,
                new JwtTokenService(null, this.options),
                new CryptoService());

            try
            {
                string token = await userService.CheckAnswerAsync(leadingQuestionModel).ConfigureAwait(false);

                Assert.True(testsMethodResult.Exception == null);
                Assert.True(!token.IsNullOrEmpty());
            }
            catch (Exception ex)
            {
                Assert.True(testsMethodResult.Exception != null);
                Assert.Equal(ex.GetType(), testsMethodResult.Exception.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsChangePasswordData))]
        public async void ChangePassword_Test(int userId, ChangePasswordModel changePasswordModel, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetAsync(2)).Returns(Task.FromResult(this.User));

            userRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                new Mock<IOptions<Settings>>().Object,
                this.localizer,
                new Mock<IJwtTokenService>().Object,
                new CryptoService());

            try
            {
                await userService.ChangePasswordAsync(userId, changePasswordModel).ConfigureAwait(false);
                Assert.Null(testsMethodResult.Exception);
            }
            catch (Exception ex)
            {
                Assert.NotNull(testsMethodResult.Exception);
                Assert.Equal(testsMethodResult.Exception.GetType(), ex.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsChangeUsernameData))]
        public async void ChangeUsername_Test(int userId, ChangeUsernameModel changeUsernameModel, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetAsync(2)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetUserAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetUserAsync("a")).Returns(Task.FromResult(new User()));

            userRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                new Mock<IOptions<Settings>>().Object,
                this.localizer,
                new Mock<IJwtTokenService>().Object,
                new CryptoService());

            try
            {
                await userService.ChangeUsernameAsync(userId, changeUsernameModel).ConfigureAwait(false);
                Assert.Null(testsMethodResult.Exception);
            }
            catch (Exception ex)
            {
                Assert.NotNull(testsMethodResult.Exception);
                Assert.Equal(testsMethodResult.Exception.GetType(), ex.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsResetAccountData))]
        public async void ResetAccount_Test(int userId, string password, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetAsync(2)).Returns(Task.FromResult(this.User));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                new Mock<IOptions<Settings>>().Object,
                this.localizer,
                new Mock<IJwtTokenService>().Object,
                new CryptoService());

            try
            {
                await userService.ResetAccountAsync(userId, password).ConfigureAwait(false);
                Assert.Null(testsMethodResult.Exception);
            }
            catch (Exception ex)
            {
                Assert.NotNull(testsMethodResult.Exception);
                Assert.Equal(testsMethodResult.Exception.GetType(), ex.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }

        [Theory]
        [ClassData(typeof(UserServiceTestsDeleteAccountData))]
        public async void DeleteAccount_Test(int userId, string password, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetAsync(2)).Returns(Task.FromResult(this.User));

            userRepository.Setup(x => x.DeleteRowAsync(It.IsAny<int>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                new Mock<IOptions<Settings>>().Object,
                this.localizer,
                new Mock<IJwtTokenService>().Object,
                new CryptoService());

            try
            {
                await userService.DeleteAccountAsync(userId, password).ConfigureAwait(false);
                Assert.Null(testsMethodResult.Exception);
            }
            catch (Exception ex)
            {
                Assert.NotNull(testsMethodResult.Exception);
                Assert.Equal(testsMethodResult.Exception.GetType(), ex.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }
    }
}
