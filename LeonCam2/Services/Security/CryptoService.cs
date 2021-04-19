// CryptoService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class CryptoService : ICryptoService
    {
        public string GetSHA512Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            using SHA512 sHA512 = new SHA512Managed();
            return BitConverter.ToString(sHA512.ComputeHash(Encoding.Default.GetBytes(input))).Replace("-", string.Empty);
        }
    }
}
