// ICameraService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Cameras
{
    using System.Threading.Tasks;
    using LeonCam2.Models.Cameras;

    public interface ICameraService
    {
        Task AddCameraAsync(CameraModel cameraModel);
    }
}
