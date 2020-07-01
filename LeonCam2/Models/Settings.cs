// Settings.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    public class Settings
    {
        public string JwtKey { get; set; }

        public int JwtTokenLifeTimeInHours { get; set; } = 24;

        public int BlockTimeInMinutes { get; set; } = 15;

        public int MaxNumberOfLoginAttempts { get; set; } = 3;

        public int BlackListControlIntervalInHours { get; set; } = 24;
    }
}
