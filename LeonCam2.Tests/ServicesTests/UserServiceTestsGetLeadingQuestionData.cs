// UserServiceTestsGetLeadingQuestionData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;

    public class UserServiceTestsGetLeadingQuestionData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "test";
        private static readonly string InvalidTestUser = "testInvalid";
        private static readonly string UsernameError = "Username cannot be empty";
        private static readonly string QuestionError = "Leading question is empty";

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                TestUser,
                new TestsMethodResult() { Result = "Question" },
            };

            yield return new object[]
            {
                InvalidTestUser,
                new TestsMethodResult() { Exception = new InternalException(QuestionError) },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
