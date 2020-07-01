// Settings.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    public class Settings
    {
        public string JwtKey { get; set; }

        public double JwtTokenLifeTimeInHours { get; set; } = 24;

        public double BlockTimeInMinutes { get; set; } = 15;

        public int MaxNumberOfLoginAttempts { get; set; } = 3;

        public double BlackListControlIntervalInHours { get; set; } = 24;
    }
}
