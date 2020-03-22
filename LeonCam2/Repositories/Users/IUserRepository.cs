// IUserRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories
{
    using System.Threading.Tasks;
    using LeonCam2.Models;

    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
