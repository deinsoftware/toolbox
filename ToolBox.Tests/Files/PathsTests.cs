using System;
using Xunit;
using System.Collections.Generic;
using System.IO;
using Moq;
using ToolBox.Platform;

namespace ToolBox.Files.Tests
{
    public class PathsTests
    {
        readonly Mock<ICommandSystem> _commandSystemMock;
        readonly ICommandSystem _commandSystem;
        readonly string _userFolder;
        readonly Mock<IFileSystem> _fileSystemMock;
        readonly IFileSystem _fileSystem;

        public PathsTests()
        {
            //Arrange
            _commandSystemMock = new Mock<ICommandSystem>(MockBehavior.Strict);
            this._commandSystem = _commandSystemMock.Object;

            _fileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            this._fileSystem = _fileSystemMock.Object;

            _commandSystemMock
                .Setup(cs => cs.GetHomeFolder(It.Is<string>(s => s == "~")))
                .Returns("/Users/user");
            this._userFolder = _commandSystem.GetHomeFolder("~");
        }

        [Fact]
        public void PathConfigurator_WhenCommandSystemIsNull_ReturnsException()
        {
            //Arrange
            Action creator = () => new PathsConfigurator(null, _fileSystem);
            //Act/Assert
            Assert.Throws<ArgumentException>(creator);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void PathConfigurator_WhenFileSystemIsNull_ReturnsException()
        {
            //Arrange
            Action creator = () => new PathsConfigurator(_commandSystem, null);
            //Act/Assert
            Assert.Throws<ArgumentException>(creator);
            _fileSystemMock.VerifyAll();
        }
        
        [Theory]
        [InlineData(@"foo/bar"     , "~", "foo", "bar")]
        [InlineData(@"foo/bar.dll" , "~", "foo", "bar.dll")]
        public void Combine_WhenCalls_ReturnsCombinedPath(string expected, params string[] paths)
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);

            string expectedResult = Path.Combine(paths);
            _fileSystemMock
                .Setup(fs => fs.PathCombine(It.IsAny<string[]>()))
                .Returns(expectedResult);
            expectedResult = Path.Combine(_userFolder, expected);
            _commandSystemMock
                .Setup(cs => cs.GetHomeFolder(It.IsAny<string>()))
                .Returns(expectedResult);
            
            //Act
            var result = creator.Combine(paths);
            //Assert
            Assert.Equal(expectedResult, result);
            _fileSystemMock.VerifyAll();
            _commandSystemMock.VerifyAll();
        }

        [Fact]
        public void GetFileName_WhenFilePathIsNull_ReturnsException()
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);
            //Act
            Action result = () => creator.GetFileName(null);
            //Assert
            Assert.Throws<ArgumentException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact(Skip="It's System Functionality")]
        public void GetFileName_WhenCalls_NotImplemented()
        {
            throw new NotImplementedException();
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
                .Setup(fs => fs.DirectoryExists(
                    It.Is<string>(s => s == path))
                )
                .Returns(true);
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
                .Setup(fs => fs.DirectoryExists(
                    It.Is<string>(s => s == path))
                )
                .Returns(false);

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
                .Setup(fs => fs.DirectoryExists(
                    It.Is<string>(s => s == path))
                )
                .Returns(true);
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

        [Fact]
        public void GetDirectoryName_WhenPathIsNull_ReturnsException()
        {
            //Arrange
            PathsConfigurator creator = new PathsConfigurator(_commandSystem, _fileSystem);
            //Act
            Action result = () => creator.GetDirectoryName(null);
            //Assert
            Assert.Throws<ArgumentException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact(Skip="It's System Functionality")]
        public void GetDirectoryName_WhenCalls_NotImplemented()
        {
            throw new NotImplementedException();
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
                .Setup(fs => fs.DirectoryExists(
                    It.Is<string>(s => s == path))
                )
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(
                    It.IsAny<string>(), It.IsAny<string>())
                )
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
                .Setup(fs => fs.DirectoryExists(
                    It.Is<string>(s => s == path))
                )
                .Returns(false);

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
                .Setup(fs => fs.DirectoryExists(
                    It.Is<string>(s => s == path))
                )
                .Returns(true);
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

        
    }
}