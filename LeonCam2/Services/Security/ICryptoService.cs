// ICryptoService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Security
{
    public interface ICryptoService
    {
        string Hash(string input);

        string Encrypt(byte[] input, byte[] key);

        string Decrypt(byte[] input, byte[] key);
    }
}
