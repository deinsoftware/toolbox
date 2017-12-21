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
        private static ICommand _cmd;
        private static string _userFolder;

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
                    expectedResult = _userFolder + @"\" + expectedWinResult;
                    break;
                case "mac":
                    expectedResult = _userFolder + @"/" + expectedMacResult;
                    break;
            }

            //Act
            var result = System.Paths.Combine(paths);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(
            new[] {
                "_Folder1",
                "_Folder2",
                "_Folder3"
            }
            , "_*")
        ]
        [InlineData(
            new[] {
                "_Folder1",
                "_Folder2",
                "_Folder3",
                "Folder1",
                "Folder2",
                "Folder3"
            }
            , null)
        ]
        public void GetDirectories_WhenCalls_ReturnsDirectoriesList(string[] expectedDirectories, string filter)
        {
            //Arrange
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "Test");
            List<string> expectedResult = new List<string>();
            foreach (var directory in expectedDirectories)
            {
                expectedResult.Add(System.Paths.Combine(_userFolder, "Test", directory));
            }

            //Act
            var result = System.Paths.GetDirectories(path, filter);
            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}