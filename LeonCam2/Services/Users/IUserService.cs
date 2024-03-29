﻿// IUserService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Users
{
    using System.Threading.Tasks;
    using LeonCam2.Models.Users;

    public interface IUserService
    {
        Task<string> GetLeadingQuestionAsync(string username);

        /// <summary>
        /// Login method.
        /// </summary>
        /// <param name="loginModel">Model to sign in.</param>
        /// <returns>JWT token.</returns>
        Task<string> LoginAsync(LoginModel loginModel);

        void Logout(string token);

        Task RegisterAsync(RegisterModel registerModel);

        Task<string> CheckAnswerAsync(LeadingQuestionModel leadingQuestionModel);

        Task ChangeUsernameAsync(int userId, ChangeUsernameModel changeUsernameModel);

        Task ChangePasswordAsync(int userId, ChangePasswordModel changePasswordModel);

        Task ResetAccountAsync(int userId, string password);

        Task DeleteAccountAsync(int userId, string password);
    }
}
