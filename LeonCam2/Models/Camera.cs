// Camera.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Camera : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string IPAddress { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
