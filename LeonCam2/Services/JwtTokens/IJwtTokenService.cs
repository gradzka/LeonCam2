// IJwtTokenService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.JwtTokens
{
    using System.Security.Claims;

    public interface IJwtTokenService
    {
        string CreateToken(int userId);

        void AddTokenToBlackList(string token);

        bool CheckIfTokenOnBlackList(string token);

        int RemoveInvalidTokensFromBlackList();

        ClaimsPrincipal ValidateToken(string token);
    }
}
