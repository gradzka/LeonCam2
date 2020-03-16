// LoginModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
