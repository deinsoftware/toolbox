using System;
using Xunit;
using ToolBox;

namespace ToolBox.Validations.Tests
{
    public class NumericTests
    {
        public NumericTests()
        {

        }

        [Theory]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("1")]
        public void IsNumber_WhenIsNumber_ReturnsTrue(string value)
        {
            //Act
            var result = Validations.Number.IsNumber(value);
            //Assert
            Assert.True(result, $"{value} is a Valid Number");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("*")]
        [InlineData("")]
        public void IsNumber_WhenIsNotANumber_ReturnsFalse(string value)
        {
            //Act
            var result = Validations.Number.IsNumber(value);
            //Assert
            Assert.False(result, $"{value} is not a Valid Number");
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 2, 3)]
        public void IsNumber_WhenNumberIsOnRange_ReturnsTrue(int min, int value, int max)
        {
            //Act
            var result = Validations.Number.IsOnRange(min, value, max);
            //Assert
            Assert.True(result, $"{value} is On Range");
        }

        [Theory]
        [InlineData(1, 0, 2)]
        [InlineData(1, 3, 2)]
        public void IsNumber_WhenNumberIsOutOfRange_ReturnsFalse(int min, int value, int max)
        {
            //Act
            var result = Validations.Number.IsOnRange(min, value, max);
            //Assert
            Assert.False(result, $"{value} is Out of Range");
        }
    }

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
