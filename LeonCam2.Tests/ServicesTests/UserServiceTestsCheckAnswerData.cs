// UserServiceTestsCheckAnswerData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;

    public class UserServiceTestsCheckAnswerData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "test";
        private static readonly string InvalidTestUser = "testInvalid";
        private static readonly string LeadingQuestionModel = "leadingQuestionModel";
        private static readonly string UsernameError = "Username cannot be empty";
        private static readonly string AnswerError = "Answer cannot be empty";

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
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = null },
                new TestsMethodResult() { Exception = new ArgumentException(AnswerError) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = string.Empty },
                new TestsMethodResult() { Exception = new ArgumentException(AnswerError) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = null, Answer = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = string.Empty, Answer = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = InvalidTestUser, Answer = InvalidTestUser },
                new TestsMethodResult() { Exception = new InternalException("Inproper username") },
            };

            yield return new object[]
            {
                new LeadingQuestionModel() { Username = TestUser, Answer = InvalidTestUser },
                new TestsMethodResult() { Exception = new InternalException("Invalid answer") },
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
