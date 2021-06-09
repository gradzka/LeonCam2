// CamerasController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Extensions;
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
        private readonly ICameraService cameraService;

        public CamerasController(ICameraService cameraService)
        {
            this.cameraService = cameraService;
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

            await this.cameraService.AddCameraAsync(camera, this.GetLoggedUserId()).ConfigureAwait(false);

            return this.Ok();
        }
    }
}