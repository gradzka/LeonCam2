// Camera.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    public class Camera : BaseEntity
    {
        public string Name { get; set; }

        public string IPAddress { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }
    }
}
