// CameraModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Cameras
{
    using System.ComponentModel.DataAnnotations;

    public class CameraModel : CameraBaseModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}