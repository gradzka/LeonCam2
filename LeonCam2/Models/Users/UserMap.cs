// UserMap.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using Dapper.FluentMap.Mapping;

    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            this.Map(x => x.Id).ToColumn("UserId");
        }
    }
}
