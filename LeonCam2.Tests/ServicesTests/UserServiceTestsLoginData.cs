﻿// UserServiceTestsLoginData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Models;
    using LeonCam2.Models.Users;

    public class UserServiceTestsLoginData : IEnumerable<object[]>
    {
        private static readonly string TestUser = "test";
        private static readonly string InvalidTestUser = "testInvalid";
        private static readonly string LoginModel = "loginModel";
        private static readonly string InproperLoginDataInfo = "Inproper login data";

        private readonly TestsMethodResult resultWithInternalException;

        public UserServiceTestsLoginData()
        {
            this.resultWithInternalException = new TestsMethodResult() { Exception = new InternalException(InproperLoginDataInfo), Result = false };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                new TestsMethodResult() { Exception = new ArgumentNullException(LoginModel), Result = false },
            };

            yield return new object[]
            {
                new LoginModel(),
                this.resultWithInternalException,
            };

            yield return new object[]
            {
                new LoginModel() { Username = TestUser, Password = null },
                this.resultWithInternalException,
            };

            yield return new object[]
            {
                new LoginModel() { Username = TestUser, Password = string.Empty },
                this.resultWithInternalException,
            };

            yield return new object[]
            {
                new LoginModel() { Username = null, Password = TestUser },
                this.resultWithInternalException,
            };

            yield return new object[]
            {
                new LoginModel() { Username = string.Empty, Password = TestUser },
                this.resultWithInternalException,
            };

            yield return new object[]
            {
                new LoginModel() { Username = InvalidTestUser, Password = InvalidTestUser },
                this.resultWithInternalException,
            };

            yield return new object[]
            {
                new LoginModel() { Username = TestUser, Password = TestUser },
                new TestsMethodResult() { Result = true },
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
