// Camera.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.DB
{
    using System.ComponentModel.DataAnnotations;

    public class Camera : BaseEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Ip { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
