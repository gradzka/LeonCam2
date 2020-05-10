// UserServiceTestsRegisterData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;

    public class UserServiceTestsRegisterData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "newtest";
        private static readonly string AlreadyUsedTestUser = "test";
        private static readonly string RegisterModel = "registerModel";

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
                new TestsMethodResult() { Exception = new ArgumentException("Username cannot be empty"), Result = false },
            };

            yield return new object[]
{
                new RegisterModel() { Username = AlreadyUsedTestUser, Password = TestUser, RepeatedPassword = TestUser },
                new TestsMethodResult() { Exception = new InternalException("Username is already used"), Result = false },
};

            yield return new object[]
            {
                new RegisterModel() { Username = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException("Password cannot be empty"), Result = false },
            };

            yield return new object[]
            {
                new RegisterModel() { Username = TestUser, Password = string.Empty, RepeatedPassword = TestUser },
                new TestsMethodResult() { Exception = new ArgumentException("Passwords must be the same"), Result = false },
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
