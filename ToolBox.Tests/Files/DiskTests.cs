using System;
using Xunit;
using ToolBox;
using ToolBox.System;
using System.Collections.Generic;
using System.Collections;
using Moq;
using ToolBox.Platform;
using System.IO;
using System.Linq;

namespace ToolBox.Files.Tests
{
    public class DiskTests
    {
        private Mock<ICommandSystem> _commandSystemMock;
        private static ICommandSystem _commandSystem;
        private static string _userFolder;
        private Mock<IFileSystem> _fileSystemMock;
        private IFileSystem _fileSystem;

        public DiskTests()
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

        [Fact(Skip="It Calls CopyDirectories and CopyFiles")]
        public void CopyAll_WhenCalls_NotImplemented()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CopyAll_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            string sourcePath = Path.Combine(_userFolder, "NotExist");
            string destinationPath = Path.Combine(_userFolder, "NotMatters");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(false);

            //Act
            Action result = () => creator.CopyAll(sourcePath, destinationPath);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenDestinationExists_CopyFolders()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/Exist" });
            
            creator.CopyDirectories(sourcePath, destinationPath);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenDestinationNotExists_CreateFolders()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/NotExist" });
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s.Contains("NotExist"))))
                .Returns(false);
            _fileSystemMock
                .Setup(fs => fs.CreateDirectory(It.IsAny<string>()))
                .Verifiable();
            
            creator.CopyDirectories(sourcePath, destinationPath);
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenSourceIsEmpty_CopyAnything()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(sourcePath, null, SearchOption.AllDirectories))
                .Returns(Enumerable.Empty<string>);
            
            creator.CopyDirectories(sourcePath, destinationPath);
            _fileSystemMock.Verify(fs => fs.DirectoryExists(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            string sourcePath = Path.Combine(_userFolder, "NotExist");
            string destinationPath = Path.Combine(_userFolder, "NotMatters");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(false);

            //Act
            Action result = () => creator.CopyDirectories(sourcePath, destinationPath);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenDestinationFolderExists_CopyFiles()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetFiles(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/Exist/File1.txt" });
            _fileSystemMock
                .Setup(fs => fs.GetDirectoryName(It.IsAny<string>()))
                .Returns("/Users/user/Source/Exist/");
            _fileSystemMock
                .Setup(fs => fs.CopyFile(It.IsAny<string>(), It.IsAny<string>(), false))
                .Verifiable();
            
            creator.CopyFiles(sourcePath, destinationPath);
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(0));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenDestinationFolderNotExists_CreateFoldersAndCopyFiles()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetFiles(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/NotExist/File1.txt" });
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s.Contains("NotExist"))))
                .Returns(false);
            _fileSystemMock
                .Setup(fs => fs.GetDirectoryName(It.Is<string>(s => s.Contains("NotExist"))))
                .Returns("/Users/user/Destination/NotExist/");
            _fileSystemMock.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Verifiable();
            _fileSystemMock.Setup(fs => fs.CopyFile(It.IsAny<string>(), It.IsAny<string>(), false)).Verifiable();
            
            creator.CopyFiles(sourcePath, destinationPath);
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenSourceIsEmpty_CopyAnything()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetFiles(sourcePath, null, SearchOption.AllDirectories))
                .Returns(Enumerable.Empty<string>);
            
            creator.CopyFiles(sourcePath, destinationPath);
            _fileSystemMock.Verify(fs => fs.DirectoryExists(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            string sourcePath = Path.Combine(_userFolder, "NotExist");
            string destinationPath = Path.Combine(_userFolder, "NotMatters");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(false);

            //Act
            Action result = () => creator.CopyFiles(sourcePath, destinationPath);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void DeleteAll_WhenCalls_DeleteFilesAndFolders()
        {
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            string path = Path.Combine(_userFolder, "Exist");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == path)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.DeleteDirectory(It.IsAny<string>(), true))
                .Verifiable();

            //Act
            creator.DeleteAll(path, true);
            //Assert
            _fileSystemMock.VerifyAll();
        }
        
        [Fact]
        public void DeleteAll_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_commandSystem, _fileSystem);

            string path = Path.Combine(_userFolder, "NotExist");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == path)))
                .Returns(false);

            //Act
            Action result = () => creator.DeleteAll(path, true);
            //Assert
            Assert.Throws<DirectoryNotFoundException>(result);
            _fileSystemMock.VerifyAll();
        }
    }
}