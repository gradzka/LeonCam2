// CameraServiceTestsAddCameraData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Enums.Messages.Services;
    using LeonCam2.Enums.Services;
    using LeonCam2.Models;
    using LeonCam2.Models.Cameras;
    using LeonCam2.Models.Users;
    using LeonCam2.Services.Cameras;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    public class CameraServiceTestsAddCameraData : IEnumerable<object[]>
    {
        private readonly StringLocalizer<CameraService> localizer;

        public CameraServiceTestsAddCameraData()
        {
            this.localizer = new StringLocalizer<CameraService>(
                new ResourceManagerStringLocalizerFactory(
                    Options.Create(new LocalizationOptions { ResourcesPath = "Resources" }),
                    NullLoggerFactory.Instance));
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                0,
                new TestsMethodResult() { Exception = new ArgumentNullException("cameraModel"), Result = false },
            };

            yield return new object[]
            {
                new CameraModel() { Description = string.Empty },
                0,
                new TestsMethodResult() { Exception = new ArgumentException(this.localizer[nameof(CameraServiceMessage.DescriptionCannotBeEmpty)], nameof(CameraModel.Description)) },
            };

            yield return new object[]
            {
                new CameraModel() { Description = "Description", Ip = "192.168.1.2", Login = "Login", Password = "Password" },
                0,
                new TestsMethodResult() { Exception = new UnauthorizedAccessException(this.localizer[nameof(CameraServiceMessage.InvalidUserId)]) },
            };

            yield return new object[]
            {
                new CameraModel() { Description = "Description", Ip = "192.168.1.2", Login = "Login", Password = "Password" },
                2,
                new TestsMethodResult(),
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
