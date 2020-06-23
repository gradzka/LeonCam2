// JwtTokenFilter.cs by Gradzka & Kazimierczak

namespace LeonCam2.Filters.AuthorizationFilters
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using LeonCam2.Models;
    using LeonCam2.Services.JwtTokens;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// https://zaven.co/blog/user-authentication-asp-net-web-api-2-rsa-jwt-tokens-part-4/ .
    /// </summary>
    public class JwtTokenFilter : Attribute, IAsyncAuthorizationFilter
    {
        private static readonly string InvalidAuthorizationScheme = "Invalid Authorization Scheme";
        private static readonly string InvalidToken = "Invalid Token";
        private static readonly string MissingAutorizationHader = "Missing autorization header";
        private static readonly string MissingToken = "Missing Token";

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

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

            bool correctToken = jwtTokenService.ValidateToken(authorization.Parameter);
            if (!correctToken)
            {
                throw new InternalException(InvalidToken);
            }

            return Task.CompletedTask;
        }
    }
}
