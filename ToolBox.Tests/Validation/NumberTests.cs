using System;
using Xunit;
using ToolBox;

namespace ToolBox.Validations.Tests
{
    public class NumberTests
    {
        public NumberTests()
        {
            //Arrange
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
        public void IsOnRange_WhenNumberIsOnRange_ReturnsTrue(int min, int value, int max)
        {
            //Act
            var result = Validations.Number.IsOnRange(min, value, max);
            //Assert
            Assert.True(result, $"{value} is On Range");
        }

        [Theory]
        [InlineData(1, 0, 2)]
        [InlineData(1, 3, 2)]
        public void IsOnRange_WhenNumberIsOutOfRange_ReturnsFalse(int min, int value, int max)
        {
            //Act
            var result = Validations.Number.IsOnRange(min, value, max);
            //Assert
            Assert.False(result, $"{value} is Out of Range");
        }
    }
}
