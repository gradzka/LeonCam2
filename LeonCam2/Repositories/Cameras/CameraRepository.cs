// CameraRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories.Cameras
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using LeonCam2.Models.DB;

    public class CameraRepository : GenericRepository<Camera>, ICameraRepository
    {
        private readonly IDbConnection dbConnection;

        public CameraRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Camera>> GetUserCamerasAsync(int userId)
        {
            return await this.dbConnection.QueryAsync<Camera>($"SELECT * FROM {this.TableName} WHERE UserId=@UserId", new { UserId = userId });
        }
    }
}
