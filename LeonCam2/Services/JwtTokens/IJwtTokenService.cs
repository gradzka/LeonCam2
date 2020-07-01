// IJwtTokenService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.JwtTokens
{
    public interface IJwtTokenService
    {
        string CreateToken(int userId);

        void AddTokenToBlackList(string token);

        bool CheckIfTokenOnBlackList(string token);

        int RemoveInvalidTokensFromBlackList();

        bool ValidateToken(string token);
    }
}
