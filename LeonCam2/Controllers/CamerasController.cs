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

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetAsync(int id) => this.Ok(await this.cameraService.GetAsync(id, this.GetLoggedUserId()));

        [HttpGet("GetUserCameras")]
        public async Task<IActionResult> GetUserCamerasAsync() => this.Ok(await this.cameraService.GetUserCamerasAsync(this.GetLoggedUserId()));

        [HttpPost("AddCamera")]
        public async Task<IActionResult> AddCamera([FromBody]CameraModel camera)
        {
            await this.cameraService.AddCameraAsync(camera, this.GetLoggedUserId()).ConfigureAwait(false);

            return this.Ok();
        }

        [HttpPost("EditCamera")]
        public async Task<IActionResult> EditCamera([FromBody]CameraEditModel camera)
        {
            await this.cameraService.EditCameraAsync(camera, this.GetLoggedUserId()).ConfigureAwait(false);

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