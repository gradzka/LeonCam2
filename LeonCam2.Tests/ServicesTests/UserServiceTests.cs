﻿// UserServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class UserServiceTests
    {
        private static readonly string JwtKey = "SecretSecret key";
        private static readonly string TestUser = "test";
        private static readonly string TestPassword = "51A670987F4C067329A37CD8C8A493C38F49A0FE8BA1EE67FCB830EA681332F69FC7DB87AA4627E3A2735798F5DCC209A1D90CCA0C95DECA685F3CC7E6AD72E6";
        private static readonly string TestDate = "2020-03-29 15:00:42.9685001";

        public UserServiceTests()
        {
            this.User = new User()
            {
                Username = TestUser,
                Password = TestPassword,
                LeadingQuestion = null,
                LastLoginAttemptDate = DateTime.Parse(TestDate),
                LastLogoutDate = DateTime.Parse(TestDate),
                AccessFailedCount = 0,
                Id = 1,
                CreationDate = DateTime.Parse(TestDate),
                ModifiedDate = DateTime.Parse(TestDate),
            };
        }

        public User User { get; set; }

        [Theory]
        [ClassData(typeof(UserServiceTestsLoginData))]
        public async void Login_Test(LoginModel loginModel, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetByUsernameAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetByUsernameAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.UpdateAsync(It.IsAny<User>()));

            Settings settings = new Settings() { JwtKey = JwtKey, MaxNumberOfLoginAttempts = 1 };
            var options = new Mock<IOptions<Settings>>();
            options.Setup(x => x.Value).Returns(settings);

            var userService = new UserService(
                userRepository: userRepository.Object,
                logger: new Mock<ILogger<UserService>>().Object,
                settings: options.Object);

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
            userRepository.Setup(x => x.GetByUsernameAsync(TestUser)).Returns(Task.FromResult(this.User));
            userRepository.Setup(x => x.GetByUsernameAsync(It.Is<string>(x => x != TestUser))).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.InsertAsync(It.IsAny<User>()));

            var userService = new UserService(
                userRepository: userRepository.Object,
                logger: new Mock<ILogger<UserService>>().Object,
                settings: new Mock<IOptions<Settings>>().Object);

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
    }
}
