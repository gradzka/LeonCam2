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
    }
}
