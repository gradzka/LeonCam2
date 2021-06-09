// CameraService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Cameras
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using LeonCam2.Enums.Messages.Services;
    using LeonCam2.Extensions;
    using LeonCam2.Models.Cameras;
    using LeonCam2.Models.DB;
    using LeonCam2.Repositories;
    using LeonCam2.Repositories.Cameras;
    using LeonCam2.Services.Security;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public class CameraService : ICameraService
    {
        private static readonly string RegisteringCameraInfo = "Registering camera...";

        private readonly ICameraRepository cameraRepository;
        private readonly ICryptoService cryptoService;
        private readonly IStringLocalizer<CameraService> localizer;
        private readonly IStringLocalizer regexLocalizer;
        private readonly ILogger<CameraService> logger;
        private readonly IUserRepository userRepository;

        public CameraService(
            ICameraRepository cameraRepository,
            IStringLocalizer<CameraService> localizer,
            ILogger<CameraService> logger,
            IUserRepository userRepository,
            ICryptoService cryptoService,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            this.cameraRepository = cameraRepository;
            this.localizer = localizer;
            this.logger = logger;
            this.userRepository = userRepository;
            this.cryptoService = cryptoService;
            this.regexLocalizer = stringLocalizerFactory.Create("Regex", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        public async Task AddCameraAsync(CameraModel cameraModel, int userId)
        {
            this.ValidateCameraModel(cameraModel);

            this.logger.LogInformation(RegisteringCameraInfo);

            DateTime dateTimeNow = DateTime.Now;

            User user = await this.userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException(this.localizer[nameof(CameraServiceMessage.InvalidUserId)]);
            }

            Camera camera = new Camera
            {
                Description = cameraModel.Description,
                Ip = cameraModel.Ip,
                Login = cameraModel.Login,
                Password = this.cryptoService.Encrypt(cameraModel.Password, this.GetCryptoKey(user.Username, user.Password)),
                CreationDate = dateTimeNow,
                ModifiedDate = dateTimeNow,
                UserId = userId,
            };

            await this.cameraRepository.InsertAsync(camera).ConfigureAwait(false);
        }

        public async Task UpdateCameraCryptoKeyAsync(int userId, byte[] oldCryptoKey, byte[] newCryptoKey)
        {
            User user = await this.userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException(this.localizer[nameof(CameraServiceMessage.InvalidUserId)]);
            }

            IEnumerable<Camera> cameras = await this.cameraRepository.GetUserCamerasAsync(userId);

            foreach (Camera camera in cameras)
            {
                string password = this.cryptoService.Decrypt(camera.Password, oldCryptoKey);
                camera.Password = this.cryptoService.Encrypt(password, newCryptoKey);
                camera.ModifiedDate = DateTime.Now;
                await this.cameraRepository.UpdateAsync(camera);
            }
        }

        private byte[] GetCryptoKey(string login, string password)
        {
            string part1 = this.cryptoService.GetSHA256Hash($"{password}{login}");
            string sha256Hash = this.cryptoService.GetSHA256Hash($"{part1}{login}");
            return Enumerable.Range(0, sha256Hash.Length / 2).Select(x => Convert.ToByte(sha256Hash.Substring(x * 2, 2), 16)).ToArray();
        }

        private void ValidateCameraModel(CameraModel cameraModel)
        {
            if (cameraModel == null)
            {
                throw new ArgumentNullException(nameof(cameraModel));
            }

            cameraModel.Description.VerifyNotNullOrEmpty(nameof(cameraModel.Description), this.localizer[nameof(CameraServiceMessage.DescriptionCannotBeEmpty)]);
            cameraModel.Ip.VerifyNotNullOrEmpty(nameof(cameraModel.Ip), this.localizer[nameof(CameraServiceMessage.IpCannotBeEmpty)]);
            cameraModel.Login.VerifyNotNullOrEmpty(nameof(cameraModel.Login), this.localizer[nameof(CameraServiceMessage.LoginCannotBeEmpty)]);
            cameraModel.Password.VerifyNotNullOrEmpty(nameof(cameraModel.Password), this.localizer[nameof(CameraServiceMessage.PasswordCannotBeEmpty)]);

            Regex ipv4Regex = new Regex(this.regexLocalizer[nameof(Enums.Regex.Ipv4Pattern)]);
            Regex ipv6Regex = new Regex(this.regexLocalizer[nameof(Enums.Regex.Ipv6Pattern)]);

            if (!(ipv4Regex.IsMatch(cameraModel.Ip) || ipv6Regex.IsMatch(cameraModel.Ip)))
            {
                throw new ArgumentException(this.localizer[nameof(CameraServiceMessage.IpInvalidFormat)]);
            }
        }
    }
}