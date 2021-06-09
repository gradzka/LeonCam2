// CameraServiceTestsRefreshCameraCryptoKeyData.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using LeonCam2.Enums.Messages.Services;
    using LeonCam2.Services.Cameras;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    public class CameraServiceTestsRefreshCameraCryptoKeyData : IEnumerable<object[]>
    {
        private readonly StringLocalizer<CameraService> localizer;

        public CameraServiceTestsRefreshCameraCryptoKeyData()
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
                0,
                null,
                null,
                new TestsMethodResult() { Exception = new UnauthorizedAccessException(this.localizer[nameof(CameraServiceMessage.InvalidUserId)]) },
            };

            yield return new object[]
            {
                2,
                null,
                null,
                new TestsMethodResult(),
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
