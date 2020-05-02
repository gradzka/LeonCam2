// IUserService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Users
{
    using System.Threading.Tasks;
    using LeonCam2.Models.Users;

    public interface IUserService
    {
        Task<string> GetLeadingQuestion(string username);

        /// <summary>
        /// Login method.
        /// </summary>
        /// <param name="loginModel">Model to sign in.</param>
        /// <returns>JWT token.</returns>
        Task<string> Login(LoginModel loginModel);

        Task Register(RegisterModel registerModel);

        Task<string> CheckAnswer(LeadingQuestionModel leadingQuestionModel);
    }
}
