// IJwtTokenService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.JwtTokens
{
    using System.Security.Claims;

    public interface IJwtTokenService
    {
        string CreateToken(int userId);

        void AddTokenToBlockedList(string token);

        bool CheckIfTokenOnBlockedList(string token);

        int RemoveInvalidTokensFromBlockedList();

        ClaimsPrincipal ValidateToken(string token);
    }
}
