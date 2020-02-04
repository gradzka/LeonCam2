// ExceptionMiddlewareTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.MiddlewareTests
{
    using System;
    using System.Threading.Tasks;
    using LeonCam2.Middleware;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ExceptionMiddlewareTests
    {
        [Theory]
        [InlineData(true, 500)]
        [InlineData(false, 200)]
        public async void InvokeAsync_Test(bool isException, int expectedStatusCode)
        {
            var httpContext = new DefaultHttpContext();

            var middleware = new ExceptionMiddleware(
                next: async (_) =>
            {
                if (isException)
                {
                    throw new Exception("Test");
                }
                else
                {
                    await Task.Delay(0).ConfigureAwait(false);
                }
            }, logger: new Mock<ILogger<ExceptionMiddleware>>().Object);

            await middleware.InvokeAsync(httpContext).ConfigureAwait(false);

            Assert.Equal(expectedStatusCode, httpContext.Response.StatusCode);
        }
    }
}
