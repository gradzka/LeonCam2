// CameraService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Cameras
{
    using System;
    using System.Text;
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
        private readonly ILogger<CameraService> logger;

        public CameraService(
            ICameraRepository cameraRepository,
            IStringLocalizer<CameraService> localizer,
            ILogger<CameraService> logger,
            IUserRepository userRepository,
            ICryptoService cryptoService)
        {
            this.cameraRepository = cameraRepository;
            this.localizer = localizer;
            this.logger = logger;
            this.cryptoService = cryptoService;
        }

        public async Task AddCameraAsync(CameraModel cameraModel)
        {
            this.ValidateCameraModel(cameraModel);

            this.logger.LogInformation(RegisteringCameraInfo);

            DateTime dateTimeNow = DateTime.Now;

            Camera camera = new Camera
            {
                Description = cameraModel.Description,
                Ip = cameraModel.Ip,
                Login = cameraModel.Login,
                Password = this.cryptoService.Encrypt(Encoding.UTF8.GetBytes(cameraModel.Password), this.GetCryptoKey(cameraModel.Login, cameraModel.Password)),
                CreationDate = dateTimeNow,
                ModifiedDate = dateTimeNow,
            };

            await this.cameraRepository.InsertAsync(camera).ConfigureAwait(false);
        }

        private byte[] GetCryptoKey(string login, string password)
        {
            string part1 = this.cryptoService.GetSHA512Hash($"{password}{login}");
            return Encoding.UTF8.GetBytes(this.cryptoService.GetSHA512Hash($"{part1}{login}"));
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

            Regex ipv4Regex = new Regex(this.localizer[nameof(Enums.Regex.Ipv4Pattern)]);
            Regex ipv6Regex = new Regex(this.localizer[nameof(Enums.Regex.Ipv6Pattern)]);

            if (!(ipv4Regex.IsMatch(cameraModel.Ip) || ipv6Regex.IsMatch(cameraModel.Ip)))
            {
                throw new ArgumentException(this.localizer[nameof(CameraServiceMessage.IpInvalidFormat)]);
            }
        }
    }
}
