// ChangePasswordModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordModel
    {
        private const string PasswordsAreNotTheSameError = "Passwords are not the same";

        [Required(AllowEmptyStrings = false)]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Compare(nameof(NewPassword), ErrorMessage = PasswordsAreNotTheSameError)]
        public string ConfirmNewPassword { get; set; }
    }
}
