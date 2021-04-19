﻿// User.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LeonCam2.Services.Security;

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

        public bool CheckPassword(string password)
        {
            return this.Password == new CryptoService().GetSHA512Hash($"{password}{this.Username}{this.CreationDate}");
        }
    }
}
