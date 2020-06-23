// CameraController.cs by Gradzka & Kazimierczak

namespace LeonCam2.Controllers
{
    using LeonCam2.Filters.AuthorizationFilters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [JwtTokenFilter]
    [Route("[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        [HttpGet("GetCameras")]
        public IActionResult GetCameras()
        {
            int x = 0;
            return this.Ok();
        }
    }
}
