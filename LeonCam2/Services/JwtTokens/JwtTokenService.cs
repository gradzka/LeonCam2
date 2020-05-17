// JwtTokenService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services.JwtTokens
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using LeonCam2.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class JwtTokenService : IJwtTokenService
    {
        private readonly ILogger<JwtTokenService> logger;
        private readonly Settings settings;
        private readonly HashSet<string> blackList;

        public JwtTokenService(ILogger<JwtTokenService> logger, IOptions<Settings> settings)
        {
            this.logger = logger;
            this.settings = settings.Value;
            this.blackList = new HashSet<string>();
        }

        public void AddTokenToBlackList(string token)
        {
            this.blackList.Add(token);
        }

        public bool CheckIfTokenOnBlackList()
        {
            // TODO
            return false;
        }

        public string CreateToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.settings.JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public void RemoveExpiredTokensFromBlackList()
        {
            // TOOD
        }
    }
}
