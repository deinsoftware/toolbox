using System;
using Xunit;
using Moq;
using System.IO;
using System.Linq;
using ToolBox.Platform;
using System.Reflection;
using System.Collections.Generic;
using ToolBox.Notification;

namespace ToolBox.Files.Tests
{
    public class DiskTests
    {
        readonly string _userFolder;
        readonly Mock<IFileSystem> _fileSystemMock;
        readonly IFileSystem _fileSystem;
        readonly Mock<INotificationSystem> _notificationSystemMock;
        readonly INotificationSystem _notificationSystem;

        public DiskTests()
        {
            //Arrange
            Mock<ICommandSystem> commandSystemMock = new Mock<ICommandSystem>(MockBehavior.Strict);
            ICommandSystem commandSystem = commandSystemMock.Object;

            _fileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            this._fileSystem = _fileSystemMock.Object;

            commandSystemMock
                .Setup(cs => cs.GetHomeFolder(It.Is<string>(s => s == "~")))
                .Returns("/Users/user");
            this._userFolder = commandSystem.GetHomeFolder("~");

            _notificationSystemMock = new Mock<INotificationSystem>(MockBehavior.Strict);
            this._notificationSystem = _notificationSystemMock.Object;
        }

        [Fact]
        public void DiskConfigurator_WhenFileSystemIsNull_ReturnsException()
        {
            //Arrange
            Action creator = () => new DiskConfigurator(null);
            //Act/Assert
            Assert.Throws<ArgumentException>(creator);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void FilterCreator_WhenExtensionIsNull_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);
            //Act
            Action result = () => creator.FilterCreator(false, null);
            //Assert
            Assert.Throws<ArgumentException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void FilterCreator_WhitoutFileSystemParameter_ReturnFilterListWithExtensions()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);
            //Act
            var result = creator.FilterCreator("css", "js");
            //Assert
            Assert.Equal(new string[] { @"([^\s]+(\.(?i)(css|js))$)" }, result);
            _fileSystemMock.VerifyAll();
        }

        [Theory]
        [InlineData(new string[] { }, false, "")]
        [InlineData(new string[] { @"^(?!\.).*" }, true, "")]
        [InlineData(new string[] { @"([^\s]+(\.(?i)(css|js))$)", @"^(?!\.).*" }, true, ".css", ".js")]
        [InlineData(new string[] { @"([^\s]+(\.(?i)(css|js))$)", @"^(?!\.).*" }, true, "css", "js")]
        [InlineData(new string[] { @"([^\s]+(\.(?i)(css|js))$)" }, false, "css", "js")]
        public void FilterCreator_WhenCalls_ReturnFilterList(string[] expectedResult, bool ignoreSystemFiles, params string[] extension)
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);
            //Act
            var result = creator.FilterCreator(ignoreSystemFiles, extension);
            //Assert
            Assert.Equal(expectedResult, result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void IsFilter_WhenFileIsNull_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);
            MethodInfo method = typeof(DiskConfigurator).GetMethod("IsFiltered", BindingFlags.NonPublic | BindingFlags.Instance);
            //Act
            object[] parameters = { new List<string>(new string[] { }), null };
            Action result = () => method.Invoke(creator, parameters);
            //Assert
            Assert.Throws<TargetInvocationException>(result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void IsFilter_WhenFilterIsNull_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);
            MethodInfo method = typeof(DiskConfigurator).GetMethod("IsFiltered", BindingFlags.NonPublic | BindingFlags.Instance);
            //Act
            object[] parameters = { null, "/Users/user/Source/Exist/File.txt" };
            var result = method.Invoke(creator, parameters);
            //Assert
            Assert.Equal(true, result);
            _fileSystemMock.VerifyAll();
        }

        [Theory]
        [InlineData(
            true,
            new string[] { @"([^\s]+(\.(?i)(css|js))$)" },
            "/Users/user/Source/Exist/File.css",
            "File.css"
        )]
        [InlineData(
            true,
            new string[] { @"([^\s]+(\.(?i)(css|js))$)" },
            "/Users/user/Source/Exist/File.js",
            "File.js"
        )]
        [InlineData(
            false,
            new string[] { @"([^\s]+(\.(?i)(css|js))$)" },
            "/Users/user/Source/Exist/File.txt",
            "File.txt"
        )]
        [InlineData(
            false,
            new string[] { @"([^\s]+(\.(?i)(sys))$)", @"^(?!\.).*" },
            "/Users/user/Source/Exist/.File.sys",
            ".File.sys"
        )]
        public void IsFiltered_WhenCalls_VerifyFileWithRules(bool expectedResult, string[] regexFilter, string filePath, string file)
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);
            MethodInfo method = typeof(DiskConfigurator).GetMethod("IsFiltered", BindingFlags.NonPublic | BindingFlags.Instance);

            _fileSystemMock
                .Setup(fs => fs.GetFileName(It.Is<string>(s => s == filePath)))
                .Returns(file);
            //Act
            object[] parameters = { (regexFilter == null ? null : new List<string>(regexFilter)), filePath };
            var result = method.Invoke(creator, parameters);
            //Assert
            Assert.Equal(expectedResult, result);
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyAll_WhenCalls_CallsCopyDirectoriesAndFiles()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/Exist" });
            _fileSystemMock
                .Setup(fs => fs.GetFiles(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/Exist/File1.txt" });
            _fileSystemMock
                .Setup(fs => fs.GetDirectoryName(It.IsAny<string>()))
                .Returns("/Users/user/Source/Exist/");
            _fileSystemMock
                .Setup(fs => fs.CopyFile(It.IsAny<string>(), It.IsAny<string>(), false))
                .Verifiable();
            //Act
            creator.CopyAll(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(0));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyAll_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(sourcePath, null, SearchOption.AllDirectories))
                .Returns(new[] { "/Users/user/Source/Exist" });
            //Act
            creator.CopyDirectories(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenDestinationNotExists_CreateFolders()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
            //Act
            creator.CopyDirectories(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenSourceIsEmpty_CopyAnything()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetDirectories(sourcePath, null, SearchOption.AllDirectories))
                .Returns(Enumerable.Empty<string>);
            //Act
            creator.CopyDirectories(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.Verify(fs => fs.DirectoryExists(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyDirectories_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
            //Act
            creator.CopyFiles(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(0));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenDestinationFolderNotExists_CreateFoldersAndCopyFiles()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
            //Act
            creator.CopyFiles(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenSourceIsEmpty_CopyAnything()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

            var sourcePath = Path.Combine(_userFolder, "Source");
            var destinationPath = Path.Combine(_userFolder, "Destination");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == sourcePath)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.GetFiles(sourcePath, null, SearchOption.AllDirectories))
                .Returns(Enumerable.Empty<string>);
            //Act
            creator.CopyFiles(sourcePath, destinationPath);
            //Assert
            _fileSystemMock.Verify(fs => fs.DirectoryExists(It.IsAny<string>()), Times.Exactly(1));
            _fileSystemMock.VerifyAll();
        }

        [Fact]
        public void CopyFiles_WhenPathNotFound_ReturnsException()
        {
            //Arrange
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
            DiskConfigurator creator = new DiskConfigurator(_fileSystem);

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
        public void DeleteAll_WhenCallsWithNotification_DeleteFilesAndFolders()
        {
            DiskConfigurator creator = new DiskConfigurator(_fileSystem, _notificationSystem);

            string path = Path.Combine(_userFolder, "Exist");
            _fileSystemMock
                .Setup(fs => fs.DirectoryExists(It.Is<string>(s => s == path)))
                .Returns(true);
            _fileSystemMock
                .Setup(fs => fs.DeleteDirectory(It.IsAny<string>(), true))
                .Verifiable();
            _notificationSystemMock
                .Setup(ns => ns.ShowAction(It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            //Act
            creator.DeleteAll(path, true);
            //Assert
            _fileSystemMock.VerifyAll();
            _notificationSystemMock.VerifyAll();
        }
    }
}