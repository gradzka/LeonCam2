// ControllerBaseExtensions.cs by Gradzka & Kazimierczak

namespace LeonCam2.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerBaseExtensions
    {
        public static int GetLoggedUserId(this ControllerBase controller)
        {
            return int.Parse(controller?.HttpContext?.User?.Identity?.Name);
        }
    }
}
