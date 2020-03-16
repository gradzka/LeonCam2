// User.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime LastLoginAttemptDate { get; set; }

        public DateTime? LastLogoutDate { get; set; }

        [Required]
        public int LoginAttemptCounter { get; set; }
    }
}
