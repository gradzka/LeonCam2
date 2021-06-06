// User.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string LeadingQuestion { get; set; }

        public string LeadingQuestionAnswer { get; set; }

        public DateTime LastLoginAttemptDate { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }
    }
}