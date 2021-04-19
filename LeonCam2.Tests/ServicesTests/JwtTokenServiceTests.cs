// JwtTokenServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System.Threading;
    using LeonCam2.Extensions;
    using LeonCam2.Models;
    using LeonCam2.Services.JwtTokens;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class JwtTokenServiceTests
    {
        private static readonly string JwtKey = "SecretSecret key";
        private static readonly double JwtTokenLifeTimeInHours = 0.0001;
        private static readonly string Token = "token";

        private readonly IOptions<Settings> options;

        public JwtTokenServiceTests()
        {
            var settings = new Settings() { JwtKey = JwtKey, JwtTokenLifeTimeInHours = JwtTokenLifeTimeInHours };
            var options = new Mock<IOptions<Settings>>();
            options.Setup(x => x.Value).Returns(settings);
            this.options = options.Object;
        }

        [Fact]
        public void CreateToken_Test()
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            string token = jwtTokenService.CreateToken(1);

            Assert.False(token.IsNullOrEmpty());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ValidateToken_Test(bool validToken)
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            bool result = jwtTokenService.ValidateToken(validToken ? jwtTokenService.CreateToken(1) : Token) != null;

            Assert.Equal(validToken, result);
        }

        [Fact]
        public void AddTokenToBlockedList_Test()
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            jwtTokenService.AddTokenToBlockedList(Token);

            Assert.True(jwtTokenService.CheckIfTokenOnBlockedList(Token));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckIfTokenOnBlockedList_Test(bool addToList)
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            if (addToList)
            {
                jwtTokenService.AddTokenToBlockedList(Token);
            }

            bool result = jwtTokenService.CheckIfTokenOnBlockedList(Token);

            Assert.Equal(addToList, result);
        }

        [Fact]
        public void RemoveInvalidTokensFromBlockedList_Test()
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            int result = jwtTokenService.RemoveInvalidTokensFromBlockedList();

            Assert.Equal(0, result);

            jwtTokenService.AddTokenToBlockedList(Token);
            jwtTokenService.AddTokenToBlockedList(jwtTokenService.CreateToken(1));

            result = jwtTokenService.RemoveInvalidTokensFromBlockedList();

            Assert.Equal(1, result);

            Thread.Sleep(10000);

            result = jwtTokenService.RemoveInvalidTokensFromBlockedList();

            Assert.Equal(1, result);
        }
    }
}
