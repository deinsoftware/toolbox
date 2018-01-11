using Xunit;

namespace ToolBox.Validations.Tests
{
    public class WebTests
    {
        [Theory]
        [InlineData("http://web.com.co")]
        [InlineData("http://www.web.com.co")]
        [InlineData("https://www.web.com.co")]
        [InlineData("http://www.web.com.co/index.html")]
        [InlineData("http://www.web.com.co/index.html?paramter=1")]
        [InlineData("http://www.web.com.co/index.html?paramter=1&parameter=2")]
        [InlineData("http://www.web.com.co/Controller")]
        [InlineData("http://www.web.com.co/Controller/Action")]
        public void IsUrl_WhenIsValidUrl_ReturnsTrue(string value)
        {
            //Act
            var result = Web.IsUrl(value);
            //Assert
            Assert.True(result, $"{value} is a Valid URL");
        }

        [Theory]
        [InlineData("www.web.com.co")]
        [InlineData("index.html")]
        [InlineData("D:/web/index.html")]
        public void IsUrl_WhenIsInvalidUrl_ReturnsFalse(string value)
        {
            //Act
            var result = Web.IsUrl(value);
            //Assert
            Assert.False(result, $"{value} is an Invalid URL");
        }

        [Theory]
        [InlineData("ftp://www.dein.com.co")]
        [InlineData("sftp://www.dein.com.co")]
        [InlineData("ftps://www.dein.com.co")]
        [InlineData("ftps://www.dein.com.co")]
        public void IsUrl_WhenHaveAnoherProtocol_ReturnsFalse(string value)
        {
            //Act
            var result = Web.IsUrl(value);
            //Assert
            Assert.False(result, $"{value} is an Invalid Protocol");
        }
    }
}
