// JwtTokenServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using System.Threading;
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

            Assert.False(string.IsNullOrEmpty(token));
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
        public void AddTokenToBlackList_Test()
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            jwtTokenService.AddTokenToBlackList(Token);

            Assert.True(jwtTokenService.CheckIfTokenOnBlackList(Token));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckIfTokenOnBlackList_Test(bool addToList)
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            if (addToList)
            {
                jwtTokenService.AddTokenToBlackList(Token);
            }

            bool result = jwtTokenService.CheckIfTokenOnBlackList(Token);

            Assert.Equal(addToList, result);
        }

        [Fact]
        public void RemoveInvalidTokensFromBlackList_Test()
        {
            var jwtTokenService = new JwtTokenService(
                new Mock<ILogger<JwtTokenService>>().Object,
                this.options);

            int result = jwtTokenService.RemoveInvalidTokensFromBlackList();

            Assert.Equal(0, result);

            jwtTokenService.AddTokenToBlackList(Token);
            jwtTokenService.AddTokenToBlackList(jwtTokenService.CreateToken(1));

            result = jwtTokenService.RemoveInvalidTokensFromBlackList();

            Assert.Equal(1, result);

            Thread.Sleep(10000);

            result = jwtTokenService.RemoveInvalidTokensFromBlackList();

            Assert.Equal(1, result);
        }
    }
}
