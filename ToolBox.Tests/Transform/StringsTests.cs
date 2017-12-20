using System;
using Xunit;
using ToolBox;

namespace ToolBox.Transform.Tests
{
    public class StringsTests
    {
        public StringsTests()
        {
            //Arrange
        }

        [Theory]
        [InlineData("", "", "Word")]
        [InlineData("", "Word", "Word")]
        [InlineData(" lot of words", "A lot of words", "A")]
        [InlineData("A lot of ", "A lot of words", "words")]
        [InlineData("A lot  words", "A lot of words", "of")]
        public void RemoveWords_WhenInputValues_ReturnsTextWithoutThem(string expectedResult, string oldValue, params string[] wordsToRemove)
        {
            //Act
            var result = Transform.Strings.RemoveWords(oldValue, wordsToRemove);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void RemoveWords_WhenInputProhibitedValues_ReturnsArgumentException(params string[] wordsToRemove)
        {
            //Arrange
            string oldValue = "Word";
            //Act
            Action result = ( () => Transform.Strings.RemoveWords(oldValue, wordsToRemove) );
            //Assert
            Assert.Throws<ArgumentException>(result);
        }

        [Fact]
        public void RemoveWords_WhenInputNullValues_ReturnsNullReferenceException()
        {
            //Arrange
            string oldValue = "Word";
            string[] wordsToRemove = new string[] {null};
            //Act
            Action result =( () => Transform.Strings.RemoveWords(oldValue, wordsToRemove) );
            //Assert
            Assert.Throws<NullReferenceException>(result);
        }
    }
}