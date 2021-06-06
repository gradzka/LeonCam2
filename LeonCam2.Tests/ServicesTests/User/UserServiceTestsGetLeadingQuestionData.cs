// UserServiceTestsGetLeadingQuestionData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Enums.Services;
    using LeonCam2.Models;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    public class UserServiceTestsGetLeadingQuestionData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "test";
        private static readonly string InvalidTestUser = "testInvalid";

        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTestsGetLeadingQuestionData()
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
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessage.UsernameCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                TestUser,
                new TestsMethodResult() { Result = "Question" },
            };

            yield return new object[]
            {
                InvalidTestUser,
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessage.LeadingQuestionEmpty)]) },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
