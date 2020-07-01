// JwtTokenFilter.cs by Gradzka & Kazimierczak

namespace LeonCam2.Filters.AuthorizationFilters
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using LeonCam2.Models;
    using LeonCam2.Services.JwtTokens;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class JwtTokenFilter : Attribute, IAsyncAuthorizationFilter
    {
        private static readonly string InvalidAuthorizationScheme = "Invalid Authorization Scheme";
        private static readonly string InvalidToken = "Invalid Token";
        private static readonly string MissingAutorizationHader = "Missing autorization header";
        private static readonly string MissingToken = "Missing Token";

        public bool AllowMultiple => false;

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            IJwtTokenService jwtTokenService = context.HttpContext.RequestServices.GetService(typeof(IJwtTokenService)) as IJwtTokenService;

            HttpRequest request = context.HttpContext.Request;
            if (!request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues authorizationValues))
            {
                throw new InternalException(MissingAutorizationHader);
            }

            AuthenticationHeaderValue authorization = AuthenticationHeaderValue.Parse(authorizationValues.First());

            if (authorization.Scheme != "Bearer")
            {
                throw new InternalException(InvalidAuthorizationScheme);
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                throw new InternalException(MissingToken);
            }

            bool correctToken = jwtTokenService.ValidateToken(authorization.Parameter) && !jwtTokenService.CheckIfTokenOnBlackList(authorization.Parameter);

            if (!correctToken)
            {
                throw new InternalException(InvalidToken);
            }

            return Task.CompletedTask;
        }
    }
}
