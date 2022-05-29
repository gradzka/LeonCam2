// ICameraService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Cameras
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LeonCam2.Models.Cameras;
    using LeonCam2.Models.DB;

    public interface ICameraService
    {
        Task AddCameraAsync(CameraModel cameraModel, int userId);

        Task EditCameraAsync(CameraEditModel cameraModel, int userId);

        Task<Camera> GetAsync(int id, int userId);

        Task<IEnumerable<Camera>> GetUserCamerasAsync(int userId);

        Task<bool> PingAsync(int id, int userId);

        Task UpdateCameraCryptoKeyAsync(int userId, byte[] oldCryptoKey, byte[] newCryptoKey);
    }
}