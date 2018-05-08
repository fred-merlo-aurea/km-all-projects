using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Web.Fakes;
using System.Web.Services.Fakes;
using ecn.webservice;
using ecn.webservice.classes.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ImageManagerTest
    {
        private ImageManager _manager;
        private IDisposable _shims;
        private const string SampleEcnAccessKey = "{2B1BDF1E-E365-41FA-BEDA-7BDCAF6F1D76}";
        private const string ImagesVirtualPath = "Images_VirtualPath";
        private const string FolderName = "txt/folder.jpg";
        private const string Response = "Response";
        private const int Id = 10;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
        }

        [Test]
        public void GetFolders_WithEmptyFolderName_ReturnSuccess()
        {
            // Arrange
            _manager = new ImageManager();
            InitializeFolder(true);

            // Act
            var result = _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void GetFolders_WithEmptyFolderNameAndNullUser_ReturnException()
        {
            // Arrange
            _manager = new ImageManager();
            InitializeFolder(false);

            // Act
            var result = _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void GetFolders_WithFolderName_ReturnSuccess()
        {
            // Arrange
            _manager = new ImageManager();
            InitializeFolder(true);

            // Act
            var result = _manager.GetFolders(SampleEcnAccessKey, FolderName);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void GetFolders_WithFolderNameAndNullUser_ReturnException()
        {
            // Arrange
            _manager = new ImageManager();
            InitializeFolder(false);

            // Act
            var result = _manager.GetFolders(SampleEcnAccessKey, FolderName);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void AddFolder_WithNullParentFolder_ReturnSuccess(bool directory, bool folderValue)
        {
            // Arrange
            _manager = new ImageManager();
            InitializeAddFolder(directory, folderValue);

            // Act
            var result = _manager.AddFolder(SampleEcnAccessKey, FolderName);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        public void AddFolder_WithNullParentFolderAndNullUser_ReturnException()
        {
            // Arrange
            _manager = new ImageManager();
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Response;

            // Act
            var result = _manager.AddFolder(SampleEcnAccessKey, FolderName);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetImages_WithNullFolderName_ReturnSuccess(bool exist)
        {
            // Arrange
            _manager = new ImageManager();
            InitializeGetImage(exist);

            // Act
            var result = _manager.GetImages(SampleEcnAccessKey);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void AddImage_WithNoFolderName_ReturnResponse(bool exist, bool folder)
        {
            // Arrange
            _manager = new ImageManager();
            InitializeAddImage(exist, folder);

            // Act
            var result = _manager.AddImage(SampleEcnAccessKey, new byte[] { }, FolderName);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Response));
        }

        private void Initialize()
        {
            ShimUser.GetByAccessKeyStringBoolean = (x, y) => new User()
            {
                DefaultClientID = Id,
                UserID = Id
            };
            ShimCustomer.GetByClientIDInt32Boolean = (x, y) => new Customer() { CustomerID = Id };
            var serverUtility = new ShimHttpServerUtility
            {
                MapPathString = (x) => ImagesVirtualPath
            };
            ShimWebService.AllInstances.ServerGet = (x) => serverUtility;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Response;
        }

        private void InitializeGetImage(bool exist)
        {
            Initialize();
            ShimDirectory.ExistsString = (x) => exist;
            ShimDirectory.GetFilesStringString = (x, y) => new string[] { FolderName };
            ShimFileInfo.AllInstances.NameGet = (x) => FolderName;
        }

        private void InitializeAddImage(bool exist, bool folder)
        {
            InitializeGetImage(exist);
            if (folder)
            {
                ShimDirectory.GetFilesStringString = (x, y) => new string[] { FolderName };
            }
            else
            {
                ShimDirectory.GetFilesStringString = (x, y) => new string[] { };
            }
        }

        private void InitializeAddFolder(bool directory, bool folderValue)
        {
            Initialize();
            ShimDirectory.ExistsString = (folder) =>
            {
                if (folder.Equals(ImagesVirtualPath))
                {
                    return directory;
                }
                else
                {
                    return folderValue;
                }
            };
            ShimDirectory.CreateDirectoryString = (x) => new ShimDirectoryInfo();
        }

        private void InitializeFolder(bool user)
        {
            if (user)
            {
                Initialize();
            }
            else
            {
                Initialize();
                ShimUser.GetByAccessKeyStringBoolean = (x, y) => null;
            }
            ShimDirectory.GetDirectoriesString = (x) => new string[] { FolderName };
        }
    }
}