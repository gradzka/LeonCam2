// BaseEntity.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using System;

    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
