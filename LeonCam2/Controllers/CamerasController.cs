// CamerasController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LeonCam2.Extensions;
    using LeonCam2.Filters.AuthorizationFilters;
    using LeonCam2.Models.Cameras;
    using LeonCam2.Services.Cameras;
    using Microsoft.AspNetCore.Mvc;
    using OnvifDiscovery.Interfaces;
    using OnvifDiscovery.Models;

    [JwtTokenFilter]
    [Route("[controller]")]
    [ApiController]
    public class CamerasController : ControllerBase
    {
        private readonly ICameraService cameraService;
        private readonly IDiscovery discoveryService;

        public CamerasController(ICameraService cameraService, IDiscovery discoveryService)
        {
            this.cameraService = cameraService;
            this.discoveryService = discoveryService;
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
            await this.cameraService.AddCameraAsync(camera, this.GetLoggedUserId()).ConfigureAwait(false);

            return this.Ok();
        }

        [HttpGet("Discover")]
        public async Task<IActionResult> Discover()
        {
            IEnumerable<DiscoveryDevice> onvifDevices = await this.discoveryService.Discover(1);

            return this.Ok(onvifDevices.Select(x => x.Address));
        }
    }
}