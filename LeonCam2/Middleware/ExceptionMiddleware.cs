// ExceptionMiddleware.cs by Gradzka & Kazimierczak

namespace LeonCam2.Middleware
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using LeonCam2.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Something went wrong: {ex}");
                await this.HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = nameof(HttpStatusCode.InternalServerError);

            if (exception is InternalException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
            }.ToString());
        }
    }
}
