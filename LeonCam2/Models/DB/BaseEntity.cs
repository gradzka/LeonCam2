// BaseEntity.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
