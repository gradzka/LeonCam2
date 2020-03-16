// StringExtension.cs by Gradzka & Kazimierczak

namespace LeonCam2.Extensions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class StringExtension
    {
        public static string GetSHA512Hash(this string parameter)
        {
            using SHA512 sHA512 = new SHA512Managed();
            return BitConverter.ToString(sHA512.ComputeHash(Encoding.Default.GetBytes(parameter))).Replace("-", string.Empty);
        }
    }
}
