﻿// UserServiceTestsDeleteAccountData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Enums.Services;
    using LeonCam2.Models;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    public class UserServiceTestsDeleteAccountData : IEnumerable<object[]>
    {
        private static readonly string TestPassword = "test";

        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTestsDeleteAccountData()
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
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.UserNotFound)]) },
            };

            yield return new object[]
            {
                2,
                string.Empty,
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.WrongPassword)]) },
            };

            yield return new object[]
            {
                2,
                TestPassword,
                new TestsMethodResult() { Result = true },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
