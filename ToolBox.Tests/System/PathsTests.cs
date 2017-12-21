using System;
using Xunit;
using ToolBox;
using ToolBox.System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Moq;

namespace ToolBox.System.Tests
{
    public class PathsTests
    {
        private static ICommandSystem _commandSystem;
        private static string _userFolder;
        private Mock<IFileSystem> _fileSystemMock;
        private IFileSystem _fileSystem;
        private Paths _paths;

        public PathsTests()
        {
            //Arrange
            switch (System.Platform.GetCurrent())
            {
                case "win":
                    _commandSystem = new WinCommandSystem();
                    break;
                case "mac":
                    _commandSystem = new MacCommandSystem();
                    break;
            }
            _userFolder = _commandSystem.GetUserFolder("~");

            _fileSystemMock = new Mock<IFileSystem>();
            _fileSystem = _fileSystemMock.Object;

            _paths = new Paths(_commandSystem, _fileSystem);
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
            var result = _paths.Combine(paths);
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
                expectedResult.Add(_paths.Combine(_userFolder, "xUnit", "Paths", directory));
            }
            string path = String.Empty;
            path = _paths.Combine(_userFolder, "xUnit", "Paths");

            //Act
            var result = _paths.GetDirectories(path, filter);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetDirectories_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            string path = String.Empty;
            path = _paths.Combine(_userFolder, "NotExist");

            //Act
            Action result = () => _paths.GetDirectories(path, null);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
        }

        [Fact]
        public void GetDirectories_WhenThereIsNoDirectories_ReturnsEmpty()
        {
            //Arrange
            string path = String.Empty;
            path = _paths.Combine(_userFolder, "xUnit", "Paths");

            //Act
            var result = _paths.GetDirectories(path, "FilterNotExists");
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
                expectedResult.Add(_paths.Combine(_userFolder, "xUnit", "Paths", path, file));
            }
            path = _paths.Combine(_userFolder, "xUnit", "Paths", path);

            //Act
            var result = _paths.GetFiles(path, extensionFilter);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetFiles_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            string path = String.Empty;
            path = _paths.Combine(_userFolder, "NotExist");

            //Act
            Action result = () => _paths.GetFiles(path, null);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
        }

        [Fact]
        public void GetFiles_WhenThereIsNoFiles_ReturnsEmpty()
        {
            //Arrange
            string path = String.Empty;
            path = _paths.Combine(_userFolder, "xUnit", "Paths");

            //Act
            var result = _paths.GetFiles(path, "FilterNotExists");
            //Assert
            Assert.Empty(result);
        }
    }
}