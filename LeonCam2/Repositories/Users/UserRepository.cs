// UserRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories
{
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using LeonCam2.Models;

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IDbConnection dbConnection;

        public UserRepository(IDbConnection dbConnection)
            : base(dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await this.dbConnection.QuerySingleOrDefaultAsync<User>($"SELECT * FROM {this.TableName} WHERE Username=@Username", new { Username = username });
        }

        public async Task<string> GetLeadingQuestionAsync(string username)
        {
            return await this.dbConnection.QuerySingleOrDefaultAsync<string>($"SELECT LeadingQuestion FROM {this.TableName} WHERE Username=@Username", new { Username = username });
        }
    }
}
