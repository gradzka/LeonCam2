// UserRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories
{
    using System.Data;
    using LeonCam2.Models;

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IDbConnection dbConnection;

        public UserRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
            this.dbConnection = dbConnection;
        }
    }
}
