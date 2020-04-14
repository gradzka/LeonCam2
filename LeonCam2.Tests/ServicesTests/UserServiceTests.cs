// UserServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class UserServiceTests
    {
        private static readonly string JwtKey = "SecretSecret key";
        private static readonly string TestUser = "test";
        private static readonly string TestQuestion = "Question";
        private static readonly string TestPassword = "51A670987F4C067329A37CD8C8A493C38F49A0FE8BA1EE67FCB830EA681332F69FC7DB87AA4627E3A2735798F5DCC209A1D90CCA0C95DECA685F3CC7E6AD72E6";
        private static readonly string TestDate = "2020-03-29 15:00:42.9685001";

        private readonly IOptions<Settings> options;
        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTests()
        {
            this.User = new User()
            {
                Username = TestUser,
                Password = TestPassword,
                LeadingQuestion = TestQuestion,
                LeadingQuestionAnswer = TestPassword,
                LastLoginAttemptDate = DateTime.Parse(TestDate),
                LastLogoutDate = DateTime.Parse(TestDate),
                AccessFailedCount = 0,
                Id = 1,
                CreationDate = DateTime.Parse(TestDate),
                ModifiedDate = DateTime.Parse(TestDate),
            };

            Settings settings = new Settings() { JwtKey = JwtKey, MaxNumberOfLoginAttempts = 1 };
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
                this.localizer);

            if (loginModel == null)
            {
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await userService.Login(loginModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else if (!(bool)testsMethodResult.Result)
            {
                InternalException ex = await Assert.ThrowsAsync<InternalException>(async () => await userService.Login(loginModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else
            {
                string token = await userService.Login(loginModel).ConfigureAwait(false);
                Assert.True(!string.IsNullOrEmpty(token));
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
                this.localizer);

            if (registerModel == null)
            {
                ArgumentNullException ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await userService.Register(registerModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else if (registerModel.Username == TestUser)
            {
                InternalException ex = await Assert.ThrowsAsync<InternalException>(async () => await userService.Register(registerModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else if (!(bool)testsMethodResult.Result)
            {
                ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(async () => await userService.Register(registerModel).ConfigureAwait(false)).ConfigureAwait(false);
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
            else
            {
                await userService.Register(registerModel).ConfigureAwait(false);
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
                this.localizer);

            try
            {
                string question = await userService.GetLeadingQuestion(username).ConfigureAwait(false);

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
        public async void CheckAnswer_Test(string username, string answer, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetUserAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository.Object,
                new Mock<ILogger<UserService>>().Object,
                this.options,
                this.localizer);

            try
            {
                string token = await userService.CheckAnswer(username, answer).ConfigureAwait(false);

                Assert.True(testsMethodResult.Exception == null);
                Assert.True(!string.IsNullOrEmpty(token));
            }
            catch (Exception ex)
            {
                Assert.True(testsMethodResult.Exception != null);
                Assert.Equal(ex.GetType(), testsMethodResult.Exception.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }
    }
}
