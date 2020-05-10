// Settings.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    public class Settings
    {
        public string JwtKey { get; set; }

        public int BlockTimeInMinutes { get; set; }

        public int MaxNumberOfLoginAttempts { get; set; }
    }
}
