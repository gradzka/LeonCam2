// CameraMap.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using Dapper.FluentMap.Mapping;

    public class CameraMap : EntityMap<Camera>
    {
        public CameraMap()
        {
            this.Map(x => x.Id).ToColumn("CameraId");
        }
    }
}
