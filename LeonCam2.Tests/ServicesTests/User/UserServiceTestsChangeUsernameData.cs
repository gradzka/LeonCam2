// UserServiceTestsChangeUsernameData.cs by Gradzka & Kazimierczak

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

    public class UserServiceTestsChangeUsernameData : IEnumerable<object[]>
    {
        private static readonly string TestPassword = "test";
        private static readonly string TestUser = "test";
        private static readonly string TakenUser = "a";
        private static readonly string AvailableUser = "testa";
        private static readonly string ChangeUsernameModel = "changeUsernameModel";
        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTestsChangeUsernameData()
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
                0,
                null,
                new TestsMethodResult() { Exception = new ArgumentNullException(ChangeUsernameModel) },
            };

            yield return new object[]
            {
                0,
                new ChangeUsernameModel(),
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessage.UsernameCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                0,
                new ChangeUsernameModel() { NewUsername = TestUser, Password = null },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.UserNotFound)]) },
            };

            yield return new object[]
            {
                2,
                new ChangeUsernameModel() { NewUsername = TestUser, Password = string.Empty },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.WrongPassword)]) },
            };

            yield return new object[]
            {
                2,
                new ChangeUsernameModel() { NewUsername = TestUser, Password = TestPassword },
                new TestsMethodResult() { Result = true },
            };

            yield return new object[]
            {
                2,
                new ChangeUsernameModel() { NewUsername = TakenUser, Password = TestPassword },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.UsernameAlreadyUsed)]) },
            };

            yield return new object[]
            {
                2,
                new ChangeUsernameModel() { NewUsername = AvailableUser, Password = TestPassword },
                new TestsMethodResult() { Result = true },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
