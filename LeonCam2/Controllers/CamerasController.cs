// CameraController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Filters.AuthorizationFilters;
    using LeonCam2.Models.Cameras;
    using LeonCam2.Services.Cameras;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    [JwtTokenFilter]
    [Route("[controller]")]
    [ApiController]
    public class CamerasController : ControllerBase
    {
        private readonly ILogger<CamerasController> logger;
        private readonly IStringLocalizer<CamerasController> localizer;
        private readonly ICameraService cameraService;

        public CamerasController(ICameraService cameraService, ILogger<CamerasController> logger, IStringLocalizer<CamerasController> localizer)
        {
            this.cameraService = cameraService;
            this.logger = logger;
            this.localizer = localizer;
        }

        [HttpGet("GetCamera")]
        public IActionResult GetCamera()
        {
            return this.Ok("Camera");
        }

        [HttpGet("GetCameras")]
        public IActionResult GetCameras()
        {
            return this.Ok(new string[] { "cam1", "cam2" });
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            return this.Ok(new Models.DB.User() { Username = "user" });
        }

        [HttpPost("AddCamera")]
        public async Task<IActionResult> AddCamera([FromBody]CameraModel camera)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera));
            }

            await this.cameraService.AddCameraAsync(camera).ConfigureAwait(false);

            return this.Ok();
        }
    }
}
