// ICameraRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories.Cameras
{
    using LeonCam2.Models.DB;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICameraRepository : IGenericRepository<Camera>
    {
        Task<IEnumerable<Camera>> GetUserCamerasAsync(int userId);

    }
}
