// BaseEntity.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
