// CameraModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Cameras
{
    using System.ComponentModel.DataAnnotations;

    public class CameraModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Ip { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
