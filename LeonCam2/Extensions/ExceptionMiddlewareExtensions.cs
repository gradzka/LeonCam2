// ExceptionMiddlewareExtensions.cs by Gradzka & Kazimierczak

namespace LeonCam2.Extensions
{
    using LeonCam2.Middleware;
    using Microsoft.AspNetCore.Builder;

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
