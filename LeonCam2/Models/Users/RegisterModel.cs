// RegisterModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel
    {
        private const string PasswordsAreNotTheSameError = "Passwords are not the same";

        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Compare(nameof(Password), ErrorMessage = PasswordsAreNotTheSameError)]
        public string RepeatedPassword { get; set; }
    }
}
