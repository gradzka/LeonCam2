// UserServiceTestsCheckAnswerData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Models;

    public class UserServiceTestsCheckAnswerData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "test";
        private static readonly string InvalidTestUser = "testInvalid";
        private static readonly string UsernameError = "Username cannot be empty";
        private static readonly string AnswerError = "Answer cannot be empty";

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                null,
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                TestUser,
                null,
                new TestsMethodResult() { Exception = new ArgumentException(AnswerError) },
            };

            yield return new object[]
            {
                TestUser,
                string.Empty,
                new TestsMethodResult() { Exception = new ArgumentException(AnswerError) },
            };

            yield return new object[]
            {
                null,
                TestUser,
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                string.Empty,
                TestUser,
                new TestsMethodResult() { Exception = new ArgumentException(UsernameError) },
            };

            yield return new object[]
            {
                InvalidTestUser,
                InvalidTestUser,
                new TestsMethodResult() { Exception = new InternalException("Inproper username") },
            };

            yield return new object[]
            {
                TestUser,
                InvalidTestUser,
                new TestsMethodResult() { Exception = new InternalException("Invalid answer") },
            };

            yield return new object[]
            {
                TestUser,
                TestUser,
                new TestsMethodResult() { Result = "Token" },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
