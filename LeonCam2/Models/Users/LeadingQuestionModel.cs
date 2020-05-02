// LeadingQuestionModel.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class LeadingQuestionModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
