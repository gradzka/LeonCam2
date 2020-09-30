// ChangeUsernameModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUsernameModel
    {
        [Required]
        public string NewUsername { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
