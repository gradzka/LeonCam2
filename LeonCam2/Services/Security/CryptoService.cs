// CryptoService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Security
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using LeonCam2.Enums.Messages.Services;
    using LeonCam2.Extensions;
    using Microsoft.Extensions.Localization;

    public class CryptoService : ICryptoService
    {
        private readonly IStringLocalizer<CryptoService> localizer;

        public CryptoService(IStringLocalizer<CryptoService> localizer)
        {
            this.localizer = localizer;
        }

        public string Decrypt(byte[] input, byte[] key)
        {
            input.VerifyNotNullOrEmpty(nameof(input), this.localizer[nameof(CryptoServiceMessage.InvalidInput)]);
            key.VerifyNotNullOrEmpty(nameof(key), this.localizer[nameof(CryptoServiceMessage.InvalidKey)]);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = key.Take(16).ToArray();

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(input))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string Encrypt(byte[] input, byte[] key)
        {
            input.VerifyNotNullOrEmpty(nameof(input), this.localizer[nameof(CryptoServiceMessage.InvalidInput)]);
            key.VerifyNotNullOrEmpty(nameof(key), this.localizer[nameof(CryptoServiceMessage.InvalidKey)]);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = key.Take(16).ToArray();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(input);
                        }

                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }
        
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
