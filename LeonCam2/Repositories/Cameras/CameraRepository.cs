// CameraRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories.Cameras
{
    using System.Data;
    using LeonCam2.Models.DB;

    public class CameraRepository : GenericRepository<Camera>, ICameraRepository
    {
        public CameraRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
        }
    }
}
