using System;
using Xunit;
using ToolBox;
using ToolBox.System.Command;
using System.Collections.Generic;
using System.Collections;
using System.IO;

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
            List<string> expectedResult = new List<string>();
            foreach (var directory in expectedDirectories)
            {
                expectedResult.Add(System.Paths.Combine(_userFolder, "xUnit", "Paths", directory));
            }
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "xUnit", "Paths");

            //Act
            var result = System.Paths.GetDirectories(path, filter);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetDirectories_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "NotExist");

            //Act
            Action result = () => System.Paths.GetDirectories(path, null);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
        }

        [Fact]
        public void GetDirectories_WhenThereIsNoDirectories_ReturnsEmpty()
        {
            //Arrange
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "xUnit", "Paths");

            //Act
            var result = System.Paths.GetDirectories(path, "FilterNotExists");
            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(
            "Folder3",
            new[] {
                "File3_1.csv",
                "File3_2.csv",
                "File3_3.csv"
            }
            , "*.csv")
        ]
        [InlineData(
            "Folder3",
            new[] {
                "File3_1.csv",
                "File3_1.txt",
                "File3_2.csv",
                "File3_2.txt",
                "File3_3.csv",
                "File3_3.txt"
            }
            , null)
        ]
        public void GetFiles_WhenCalls_ReturnsFileList(string path, string[] expectedFiles, string extensionFilter)
        {
            //Arrange
            List<string> expectedResult = new List<string>();
            foreach (var file in expectedFiles)
            {
                expectedResult.Add(System.Paths.Combine(_userFolder, "xUnit", "Paths", path, file));
            }
            path = System.Paths.Combine(_userFolder, "xUnit", "Paths", path);

            //Act
            var result = System.Paths.GetFiles(path, extensionFilter);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetFiles_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "NotExist");

            //Act
            Action result = () => System.Paths.GetFiles(path, null);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
        }

        [Fact]
        public void GetFiles_WhenThereIsNoFiles_ReturnsEmpty()
        {
            //Arrange
            string path = String.Empty;
            path = System.Paths.Combine(_userFolder, "xUnit", "Paths");

            //Act
            var result = System.Paths.GetFiles(path, "FilterNotExists");
            //Assert
            Assert.Empty(result);
        }
    }
}