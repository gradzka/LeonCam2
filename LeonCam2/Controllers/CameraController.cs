// CameraController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using LeonCam2.Filters.AuthorizationFilters;
    using Microsoft.AspNetCore.Mvc;

    [JwtTokenFilter]
    [Route("[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        [HttpGet("GetCameras")]
        public IActionResult GetCameras()
        {
            return this.Ok();
        }
    }
}
