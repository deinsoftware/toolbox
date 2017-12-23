using System;
using Xunit;
using ToolBox;
using ToolBox.System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Moq;
using ToolBox.Files;
using ToolBox.Platform;

namespace ToolBox.Files.Tests
{
    public class PathsTests
    {
        private Mock<ICommandSystem> _commandSystemMock;
        private static ICommandSystem _commandSystem;
        private static string _userFolder;
        private Mock<IFileSystem> _fileSystemMock;
        private IFileSystem _fileSystem;

        public PathsTests()
        {
            //Arrange
            _commandSystemMock = new Mock<ICommandSystem>(MockBehavior.Strict);
            _commandSystem = _commandSystemMock.Object;

            _fileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            _fileSystem = _fileSystemMock.Object;

            _commandSystemMock
                .Setup(cs => cs.GetHomeFolder(It.Is<string>(s => s == "~")))
                .Returns("/Users/user");
            _userFolder = _commandSystem.GetHomeFolder("~");
        }
        
        [Theory]
        [InlineData(@"foo/bar"     , "~", "foo", "bar")]
        [InlineData(@"foo/bar.dll" , "~", "foo", "bar.dll")]
        public void Combine_WhenCalls_ReturnsCombinedPath(string expected, params string[] paths)
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            string expectedResult = Path.Combine(_userFolder, expected);
            _commandSystemMock
                .Setup(cs => cs.GetHomeFolder(It.IsAny<string>()))
                .Returns(expectedResult);
            
            //Act
            var result = creator.Combine(paths);
            //Assert
            Assert.Equal(expectedResult, result);
            _commandSystemMock.VerifyAll();
        }

        [Theory]
        [InlineData(
            new[] {
                "/Users/user/Exist/_Folder1",
                "/Users/user/Exist/_Folder2",
                "/Users/user/Exist/_Folder3"
            }
            , "Exist"
            , "_*")
        ]
        [InlineData(
            new[] {
                "/Users/user/Exist/_Folder1",
                "/Users/user/Exist/_Folder2",
                "/Users/user/Exist/_Folder3",
                "/Users/user/Exist/Folder1",
                "/Users/user/Exist/Folder2",
                "/Users/user/Exist/Folder3"
            }
            , "Exist"
            , null)
        ]
        public void GetDirectories_WhenPathFound_ReturnsDirectoriesList(string[] expectedResult, string path, string filter)
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            path = Path.Combine(_userFolder, path);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedResult);

            //Act
            var result = creator.GetDirectories(path, filter);
            //Assert
            Assert.Equal(expectedResult, result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void GetDirectories_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            string path = Path.Combine(_userFolder, "NotExist");
            _fileSystemMock
                .Setup(
                    fs => fs.GetDirectories(
                        It.Is<string>(s => s == path), It.IsAny<string>()
                    )
                )
                .Throws(new DirectoryNotFoundException());

            //Act
            Action result = () => creator.GetDirectories(path, null);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void GetDirectories_WhenPathFoundButThereIsNoDirectories_ReturnsEmpty()
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            string path = Path.Combine(_userFolder, "Exist");
            _fileSystemMock
                .Setup(
                    fs => fs.GetDirectories(
                        It.Is<string>(s => s == path), It.IsAny<string>()
                    )
                )
                .Returns(new List<string>());

            //Act
            var result = creator.GetDirectories(path, "FilterNotExists");
            //Assert
            Assert.Empty(result);
            _fileSystemMock.VerifyAll();
        }

        [Theory]
        [InlineData(
            new[] {
                "/Users/user/Exist/File3_1.csv",
                "/Users/user/Exist/File3_2.csv",
                "/Users/user/Exist/File3_3.csv"
            }
            , "Exists"
            , "*.csv")
        ]
        [InlineData(
            new[] {
                "/Users/user/Exist/File3_1.csv",
                "/Users/user/Exist/File3_1.txt",
                "/Users/user/Exist/File3_2.csv",
                "/Users/user/Exist/File3_2.txt",
                "/Users/user/Exist/File3_3.csv",
                "/Users/user/Exist/File3_3.txt"
            }
            , "Exists"
            , null)
        ]
        public void GetFiles_WhenCalls_ReturnsFileList(string[] expectedResult, string path, string filter)
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            path = Path.Combine(_userFolder, path);
            _fileSystemMock
                .Setup(fs => fs.GetFiles(path, It.IsAny<string>()))
                .Returns(expectedResult);

            //Act
            var result = creator.GetFiles(path, filter);
            //Assert
            Assert.Equal(expectedResult, result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void GetFiles_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            string path = Path.Combine(_userFolder, "NotExist");
            _fileSystemMock
                .Setup(
                    fs => fs.GetFiles(
                        It.Is<string>(s => s == path), It.IsAny<string>()
                    )
                )
                .Throws(new DirectoryNotFoundException());

            //Act
            Action result = () => creator.GetFiles(path, null);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void GetFiles_WhenThereIsNoFiles_ReturnsEmpty()
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            string path = Path.Combine(_userFolder, "Exist");
            _fileSystemMock
                .Setup(
                    fs => fs.GetFiles(
                        It.Is<string>(s => s == path), It.IsAny<string>()
                    )
                )
                .Returns(new List<string>());

            //Act
            var result = creator.GetFiles(path, "FilterNotExists");
            //Assert
            Assert.Empty(result);
            _fileSystemMock.VerifyAll();
        }
    }
}