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

        public bool CheckIfTokenOnBlackList(string token)
        {
            return this.blackList.Contains(token);
        }

        public string CreateToken(int userId)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(this.settings.JwtKey);

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

        public bool ValidateToken(string token)
        {
            try
            {
                SecurityToken securityToken = new JwtSecurityToken(token);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(this.settings.JwtKey);

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),
                };

                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning($"Token not passed validation: {token}. Error: {ex}");
                return false;
            }

            return !this.CheckIfTokenOnBlackList(token);
        }
    }
}
