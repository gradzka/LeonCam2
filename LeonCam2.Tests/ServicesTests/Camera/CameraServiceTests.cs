// CameraServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Models.Cameras;
    using LeonCam2.Models.DB;
    using LeonCam2.Models.Users;
    using LeonCam2.Repositories;
    using LeonCam2.Repositories.Cameras;
    using LeonCam2.Services.Cameras;
    using LeonCam2.Services.JwtTokens;
    using LeonCam2.Services.Security;
    using LeonCam2.Services.Users;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class CameraServiceTests
    {
        private static readonly string TestUser = "test";

        private readonly StringLocalizer<CameraService> localizer;
        private readonly IStringLocalizerFactory stringLocalizerFactory;

        public CameraServiceTests()
        {
            const string TestDate = "2020-03-29 15:00:42.9685001";
            const string TestPassword = "51A670987F4C067329A37CD8C8A493C38F49A0FE8BA1EE67FCB830EA681332F69FC7DB87AA4627E3A2735798F5DCC209A1D90CCA0C95DECA685F3CC7E6AD72E6";

            this.User = new User()
            {
                Username = TestUser,
                Password = TestPassword,
                LeadingQuestion = "Question",
                LeadingQuestionAnswer = TestPassword,
                LastLoginAttemptDate = DateTime.Parse(TestDate),
                AccessFailedCount = 0,
                Id = 1,
                CreationDate = DateTime.Parse(TestDate),
                ModifiedDate = DateTime.Parse(TestDate),
            };

            this.stringLocalizerFactory = new ResourceManagerStringLocalizerFactory(
                Options.Create(new LocalizationOptions { ResourcesPath = "Resources" }),
                NullLoggerFactory.Instance);

            this.localizer = new StringLocalizer<CameraService>(this.stringLocalizerFactory);
        }

        public User User { get; set; }

        [Theory]
        [ClassData(typeof(CameraServiceTestsAddCameraData))]
        public async void AddCameraAsync_Test(CameraModel cameraModel, int userId, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetAsync(2)).Returns(Task.FromResult(this.User));

            var cameraRepository = new Mock<ICameraRepository>();
            cameraRepository.Setup(x => x.GetUserCamerasAsync(It.IsAny<int>())).Returns(Task.FromResult(Enumerable.Empty<Camera>()));
            cameraRepository.Setup(x => x.InsertAsync(It.IsAny<Camera>()));

            var cryptoService = new Mock<ICryptoService>();
            cryptoService.Setup(x => x.GetSHA256Hash(It.IsAny<string>())).Returns(string.Empty);

            var cameraService = new CameraService(
                cameraRepository.Object,
                this.localizer,
                new Mock<ILogger<CameraService>>().Object,
                userRepository.Object,
                cryptoService.Object,
                this.stringLocalizerFactory);

            try
            {
                await cameraService.AddCameraAsync(cameraModel, userId).ConfigureAwait(false);

                Assert.True(testsMethodResult.Exception == null);
            }
            catch (Exception ex)
            {
                Assert.True(testsMethodResult.Exception != null);
                Assert.Equal(ex.GetType(), testsMethodResult.Exception.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }

        [Theory]
        [ClassData(typeof(CameraServiceTestsRefreshCameraCryptoKeyData))]
        public async void RefreshCameraCryptoKeyAsync_Test(int userId, byte[] oldCryptoKey, byte[] newCryptoKey, TestsMethodResult testsMethodResult)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<User>(default));
            userRepository.Setup(x => x.GetAsync(2)).Returns(Task.FromResult(this.User));

            var cameraRepository = new Mock<ICameraRepository>();
            cameraRepository.Setup(x => x.GetUserCamerasAsync(It.IsAny<int>())).Returns(Task.FromResult(Enumerable.Empty<Camera>()));

            var cameraService = new CameraService(
                cameraRepository.Object,
                this.localizer,
                null,
                userRepository.Object,
                null,
                this.stringLocalizerFactory);

            try
            {
                await cameraService.RefreshCameraCryptoKeyAsync(userId, oldCryptoKey, newCryptoKey).ConfigureAwait(false);

                Assert.True(testsMethodResult.Exception == null);
            }
            catch (Exception ex)
            {
                Assert.True(testsMethodResult.Exception != null);
                Assert.Equal(ex.GetType(), testsMethodResult.Exception.GetType());
                Assert.Equal(testsMethodResult.Exception.Message, ex.Message);
            }
        }
    }
}