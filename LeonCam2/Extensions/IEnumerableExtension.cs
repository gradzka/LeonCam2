// IEnumerableExtension.cs by Gradzka & Kazimierczak

namespace LeonCam2.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtension
    {
        public static void VerifyNotNullOrEmpty<T>(this IEnumerable<T> parameter, string parameterName, string message)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (parameter.Count() == 0)
            {
                throw new ArgumentException(message, parameterName);
            }
        }
    }
}