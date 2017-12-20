using System;
using Xunit;
using ToolBox;
using ToolBox.System.Command;
using System.Collections.Generic;
using System.Collections;

namespace ToolBox.System.Tests
{
    public class PathsTests
    {
        private ICommand _cmd;
        private string _userFolder;
        public string UserFolder
        {
            get { return _userFolder;}
            set { _userFolder = value;}
        }
        

        public PathsTests()
        {
            //Arrange
            switch (System.Platform.GetCurrent())
            {
                case "win":
                    _cmd = new Command.WinCommand();
                    break;
                case "mac":
                    _cmd = new Command.MacCommand();
                    break;
            }
            _userFolder = _cmd.GetUserFolder("~");
        }

        [Theory]
        [InlineData(@"", @"")]
        [InlineData(@"/foo/bar", @"/foo/bar")]
        [InlineData(@"/foo/bar", @"\foo\bar")]
        [InlineData(@"/foo/bar", @"\foo/bar")]
        public void ToSlash_WhenCalls_ReturnsPathWithSlash(string expectedResult, string path)
        {
            //Act
            var result = System.Paths.ToSlash(path);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(@"", @"")]
        [InlineData(@"C:\foo\bar", @"C:/foo/bar")]
        [InlineData(@"C:\foo\bar", @"C:\foo\bar")]
        [InlineData(@"C:\foo\bar", @"C:\foo/bar")]
        public void ToBackslash_WhenCalls_ReturnsPathWithSlash(string expectedResult, string path)
        {
            //Act
            var result = System.Paths.ToBackslash(path);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(@"foo\bar"     , @"foo/bar"     , "~", "foo", "bar")]
        [InlineData(@"foo\bar.dll" , @"foo/bar.dll" , "~", "foo", "bar.dll")]
        public void Combine_WhenCalls_ReturnsCombinedPath(string expectedWinResult, string expectedMacResult, params string[] paths)
        {
            //Arrange
            string expectedResult = String.Empty;
            switch (System.Platform.GetCurrent())
            {
                case "win":
                    expectedResult = _userFolder + @"/" + expectedWinResult;
                    break;
                case "mac":
                    expectedResult = _userFolder + @"\" + expectedWinResult;
                    break;
            }

            //Act
            var result = System.Paths.Combine(paths);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(GetData(_userFolder)))]
        public void GetDirectories_WhenCalls_ReturnsDirectoriesList(List<string> expectedResult, string filter)
        {
            //Arrange
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "Test");

            //Act
            var result = System.Paths.GetDirectories(path, filter);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> GetData(string userFolder)
        {
            yield return new object[]
            {
                new List<string> {
                    System.Paths.Combine(userFolder, "_Filter1"), 
                    System.Paths.Combine(userFolder, "_Filter2"), 
                    System.Paths.Combine(userFolder, "_Filter3")
                },
                "_*"
            };

            yield return new object[]
            {
                new List<string> {
                    System.Paths.Combine(userFolder, "_Filter1"), 
                    System.Paths.Combine(userFolder, "_Filter2"), 
                    System.Paths.Combine(userFolder, "_Filter3"), 
                    System.Paths.Combine(userFolder,  "Filter1"), 
                    System.Paths.Combine(userFolder,  "Filter2"), 
                    System.Paths.Combine(userFolder,  "Filter3")
                },
                null
            };
        }
    }
}