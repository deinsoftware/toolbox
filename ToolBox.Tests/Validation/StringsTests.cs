using System;
using Xunit;
using ToolBox;

namespace ToolBox.Validations.Tests
{
    public class StringsTests
    {
        public StringsTests(){

        }

        [Theory]
        [InlineData("a")]
        [InlineData("a", "b")]
        public void IsUrl_WhenIsValidInput_ReturnsFalse(params string[] values)
        {
            //Act
            var result = Validations.Strings.SomeNullOrEmpty(values);
            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("", "a")]
        [InlineData("a", "")]
        [InlineData("", "a", "")]
        public void IsUrl_WhenIsInvalidInput_ReturnsTrue(params string[] values)
        {
            //Act
            var result = Validations.Strings.SomeNullOrEmpty(values);
            //Assert
            Assert.True(result);
        }
    }
}
