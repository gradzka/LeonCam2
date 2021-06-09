// ICryptoService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Security
{
    public interface ICryptoService
    {
        string Decrypt(string input, byte[] key);

        string Encrypt(string input, byte[] key);

        string GetSHA256Hash(string input);

        string GetSHA512Hash(string input);
    }
}