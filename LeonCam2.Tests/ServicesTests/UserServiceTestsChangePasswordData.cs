// UserServiceTestsChangePasswordData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Enums;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    public class UserServiceTestsChangePasswordData : IEnumerable<object[]>
    {
        private static readonly string TestPassword = "test";
        private static readonly string NewPassword = "testa";

        private static readonly string ChangePasswordModel = "changePasswordModel";

        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTestsChangePasswordData()
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
                new TestsMethodResult() { Exception = new ArgumentNullException(ChangePasswordModel) },
            };

            yield return new object[]
            {
                0,
                new ChangePasswordModel() { NewPassword = TestPassword, ConfirmNewPassword = NewPassword },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessages.PasswordsMustBeTheSame)]) },
            };

            yield return new object[]
            {
                0,
                new ChangePasswordModel(),
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessages.UserNotFound)]) },
            };

            yield return new object[]
            {
                2,
                new ChangePasswordModel() { OldPassword = string.Empty },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessages.WrongPassword)]) },
            };

            yield return new object[]
            {
                2,
                new ChangePasswordModel() { OldPassword = TestPassword, NewPassword = TestPassword, ConfirmNewPassword = TestPassword },
                new TestsMethodResult() { Result = true },
            };

            yield return new object[]
            {
                2,
                new ChangePasswordModel() { OldPassword = TestPassword, NewPassword = NewPassword, ConfirmNewPassword = NewPassword },
                new TestsMethodResult() { Result = true },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
