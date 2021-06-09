// UserServiceTestsRegisterData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Enums.Services;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    public class UserServiceTestsRegisterData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "newtest";
        private static readonly string AlreadyUsedTestUser = "test";
        private static readonly string RegisterModel = "registerModel";

        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTestsRegisterData()
        {
            this.localizer = new StringLocalizer<UserService>(
                new ResourceManagerStringLocalizerFactory(
                    Options.Create(new LocalizationOptions { ResourcesPath = "Resources" }),
                    NullLoggerFactory.Instance));
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                new TestsMethodResult() { Exception = new ArgumentNullException(RegisterModel), Result = false },
            };

            yield return new object[]
            {
                new RegisterModel(),
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessage.UsernameCannotBeEmpty)]), Result = false },
            };

            yield return new object[]
{
                new RegisterModel() { Username = AlreadyUsedTestUser, Password = TestUser, RepeatedPassword = TestUser },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.UsernameAlreadyUsed)]), Result = false },
};

            yield return new object[]
            {
                new RegisterModel() { Username = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessage.PasswordCannotBeEmpty)]), Result = false },
            };

            yield return new object[]
            {
                new RegisterModel() { Username = TestUser, Password = string.Empty, RepeatedPassword = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessage.PasswordsMustBeTheSame)]), Result = false },
            };

            yield return new object[]
            {
                new RegisterModel() { Username = TestUser, Password = TestUser, RepeatedPassword = TestUser },
                new TestsMethodResult() { Result = true },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
