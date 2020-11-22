// UserServiceTestsCheckAnswerData.cs by Gradzka & Kazimierczak

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

    public class UserServiceTestsCheckAnswerData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "test";
        private static readonly string InvalidTestUser = "testInvalid";
        private static readonly string LeadingQuestionModel = "leadingQuestionModel";

        private readonly StringLocalizer<UserService> localizer;

        public UserServiceTestsCheckAnswerData()
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
                new TestsMethodResult() { Exception = new ArgumentNullException(LeadingQuestionModel) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel(),
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = null },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessages.AnswerCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = string.Empty },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessages.AnswerCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = null, Answer = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = string.Empty, Answer = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(UserServiceMessages.UsernameCannotBeEmpty)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = InvalidTestUser, Answer = InvalidTestUser },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessages.InproperUsername)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = InvalidTestUser },
                new TestsMethodResult() { Exception = new InternalException(this.localizer[nameof(UserServiceMessages.InvalidAnswer)]) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = TestUser },
                new TestsMethodResult() { Result = "Token" },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
