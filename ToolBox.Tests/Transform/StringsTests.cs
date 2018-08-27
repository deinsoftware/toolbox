using System;
using Xunit;

namespace ToolBox.Transform.Tests
{
    public class StringsTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("", "\r")]
        [InlineData("", "\n")]
        [InlineData("", "\r\n")]
        public void CleanSpecialCharacters_WhenInputValues_ReturnsTextWithoutThem(string expectedResult, string oldValue)
        {
            //Act
            var result = Transform.Strings.CleanSpecialCharacters(oldValue);
            //Assert
            Assert.Equal(expectedResult, result);
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
            Action result = (() => Transform.Strings.RemoveWords(oldValue, wordsToRemove));
            //Assert
            Assert.Throws<ArgumentException>(result);
        }

        [Fact]
        public void RemoveWords_WhenInputNullValues_ReturnsNullReferenceException()
        {
            //Arrange
            string oldValue = "Word";
            string[] wordsToRemove = null;
            //Act
            Action result = (() => Transform.Strings.RemoveWords(oldValue, wordsToRemove));
            //Assert
            Assert.Throws<NullReferenceException>(result);
        }

        [Theory]
        [InlineData("Foo", "Foo", 0)]
        [InlineData("", "Foo", 1)]
        [InlineData("Foo", "Foo Bar", 0)]
        [InlineData("Bar", "Foo Bar", 1)]
        [InlineData("", "Foo Bar", 2)]
        public void GetWord_WhenInputValues_ReturnsWord(string expectedResult, string value, int wordPosition)
        {
            //Act
            var result = Transform.Strings.GetWord(value, wordPosition);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetWord_WhenInputProhibitedValues_ReturnsArgumentException()
        {
            //Arrange
            string oldValue = "Foo";
            //Act
            Action result = (() => Transform.Strings.GetWord(oldValue, -1));
            //Assert
            Assert.Throws<ArgumentException>(result);
        }

        [Theory]
        [InlineData(new string[] { "Foo" }, "Foo")]
        [InlineData(new string[] { "Foo Bar" }, "Foo Bar")]
        [InlineData(new string[] { "Foo", "Bar" }, "Foo\nBar")]
        [InlineData(new string[] { "Foo ", " Bar " }, "Foo \n Bar ")]
        public void SplitLines_WhenInputValues_ReturnEachLinesOnArray(string[] expectedResult, string value)
        {
            //Act
            string[] result = Transform.Strings.SplitLines(value);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("", "Foo", "Bar")]
        [InlineData("Foo", "Foo", "Foo")]
        [InlineData("Foo", "Foo\nBar", "Foo")]
        [InlineData("Bar", "Foo\nBar", "Bar")]
        [InlineData("", "Foo\nBar", "")]
        public void ExtractLine_WhenInputValues_ReturnsFirstLine(string expectedResult, string value, string search)
        {
            //Act
            var result = Transform.Strings.ExtractLine(value, search);
            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}