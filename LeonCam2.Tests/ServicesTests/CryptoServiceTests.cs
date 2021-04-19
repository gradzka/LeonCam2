// CryptoServiceTests.cs by Gradzka & Kazimierczak

namespace LeonCam2.Tests.ServicesTests
{
    using LeonCam2.Services.Security;
    using Xunit;

    public class CryptoServiceTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E")]
        [InlineData("test", "EE26B0DD4AF7E749AA1A8EE3C10AE9923F618980772E473F8819A5D4940E0DB27AC185F8A0E1D5F84F88BC887FD67B143732C304CC5FA9AD8E6F57F50028A8FF")]
        public void GetSHA512Hash_Test(string input, string output)
        {
            var cryptoService = new CryptoService(null);

            string result = cryptoService.GetSHA512Hash(input);

            Assert.Equal(output, result);
        }
    }
}
