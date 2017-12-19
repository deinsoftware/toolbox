using System;
using Xunit;
using ToolBox;

namespace ToolBox.Validations.Tests
{
    public class WebTests
    {
        public WebTests(){

        }

        [Theory]
        [InlineData("http://www.dein.com.co")]
        [InlineData("https://www.dein.com.co")]
        [InlineData("http://www.dein.com.co/index.html")]
        [InlineData("http://www.dein.com.co/Controller")]
        [InlineData("http://www.dein.com.co/Controller/Action")]
        public void IsUrl_WhenIsValidUrl_ReturnsTrue(string value)
        {
            //Act
            var result = Validations.Web.IsUrl(value);
            //Assert
            Assert.True(result, $"{value} is a Valid URL");
        }

        [Theory]
        [InlineData("www.dein.com.co")]
        [InlineData("index.html")]
        [InlineData("D:/web/index.html")]
        public void IsUrl_WhenIsInvalidUrl_ReturnsFalse(string value)
        {
            //Act
            var result = Validations.Web.IsUrl(value);
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
            var result = Validations.Web.IsUrl(value);
            //Assert
            Assert.False(result, $"{value} is an Invalid URL");
        }
    }
}
