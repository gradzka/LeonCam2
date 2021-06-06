// ICameraRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories.Cameras
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LeonCam2.Models.DB;

    public interface ICameraRepository : IGenericRepository<Camera>
    {
        Task<IEnumerable<Camera>> GetUserCamerasAsync(int userId);
    }
}