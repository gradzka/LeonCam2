// ICryptoService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.Security
{
    public interface ICryptoService
    {
        string GetSHA512Hash(string input);
    }
}
