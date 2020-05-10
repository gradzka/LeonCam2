// InternalException.cs by Gradzka & Kazimierczak

namespace LeonCam2.Models
{
    using System;

    public class InternalException : Exception
    {
        public InternalException()
        {
        }

        public InternalException(string message)
            : base(message)
        {
        }

        public InternalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
