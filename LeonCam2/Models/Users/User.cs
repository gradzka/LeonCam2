// User.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using System;

    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string SecurityQuestions { get; set; }

        public string Answer { get; set; }

        public string Email { get; set; }

        public DateTime LastLoginAttemptDate { get; set; }

        public DateTime LastLogoutDate { get; set; }

        public int LoginAttemptCounter { get; set; }

        public int RedAlertDeleteSettingID { get; set; }

        public int GreenAlertDeleteSettingID { get; set; }
    }
}
