using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using ADMS.ClientMethods;
using ADMS.Services.Fakes;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessClientMethods = FrameworkUAS.BusinessLogic.ClientMethods;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class NordstarTests
    {
        private readonly Northstar Northstar = new Northstar();
        private readonly string Command = "Sample Command";

        private PrivateObject _northStarPrivateObject;
        private BusinessClientMethods _clientMethodsData;
        private DataTable _dtGroup;

        private const string LogNorthstarRelationalMessageMethod = "LogNorthstarRelationalMessage";

        [Test]
        public void CreateNorthstarWebFiles_DirectoryNotExist_NoGetFilesCalled()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Northstar();
                var client = new KMPlatform.Entity.Client();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.Northstar_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = info =>
                {
                    getFilesCalled = true;
                    return null;
                };

                // Act
                testObject.CreateNorthstarWebFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeFalse();
            }
        }

        [Test]
        public void CreateNorthstarWebFiles_DirectoryExists_NoFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Northstar();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.Northstar_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new FileInfo[] { };
                };

                // Act
                testObject.CreateNorthstarWebFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateNorthstarWebFiles_DirectoryExists_PartialFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Northstar();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.Northstar_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("WEB_Group.txt"),
                        new FileInfo("nynow365 drupoal leads march 25.csv"),
                    };
                };

                // Act
                testObject.CreateNorthstarWebFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateNorthstarWebFiles_DirectoryExists_AllFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Northstar();
                var getFilesCalled = false;
                var exceptionLogged = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.Northstar_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("WEB_Group.txt"),
                        new FileInfo("WEB_Person.txt"),
                    };
                };

                ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                    (_, __, ___, _4, _5, _6) => { exceptionLogged = true; };

                ShimFtpFunctions.ConstructorStringStringString = (_, __, ___, ____) => { };
                KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.UploadStringStringBoolean = (_, __, ___, ____) => true;
                ShimClientFTP.AllInstances.SelectClientInt32 = (ftp, i) => new List<ClientFTP>()
                {
                    new ClientFTP()
                };

                // Act
                testObject.CreateNorthstarWebFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
                exceptionLogged.ShouldBeTrue();
            }
        }

        [Test]
        public void LogNorthstarRelationalMessage_WhenClientMethodsDataIsNull_ThrowsException()
        {
            // Arrange
            InitializePrivateObject();
            _clientMethodsData = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _northStarPrivateObject.Invoke(LogNorthstarRelationalMessageMethod, _clientMethodsData, _dtGroup, Command));
        }

        [Test]
        public void LogNorthstarRelationalMessage_WhenDataTableGroupIsNull_ThrowsException()
        {
            // Arrange
            InitializePrivateObject();
            _clientMethodsData = new BusinessClientMethods();
            _dtGroup = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _northStarPrivateObject.Invoke(LogNorthstarRelationalMessageMethod, _clientMethodsData, _dtGroup, Command));
        }

        [Test]
        [TestCase("Northstar_Relational_AddGroup")]
        [TestCase("Northstar_Relational_InsertPerson")]
        public void InsertGlmRelationalData_WhenClientMethodsDataAndDataTableNyNowAreNotNull_ShouldInsertData(string command)
        {
            using (ShimsContext.Create())
            {
                // Arrange
                InitializePrivateObject();
                _clientMethodsData = new BusinessClientMethods();
                _dtGroup = new DataTable();

                ShimClientMethods.AllInstances.Northstar_Relational_AddGroupDataTable = (_, dataTable) => true;
                ShimClientMethods.AllInstances.Northstar_Relational_InsertPersonDataTable = (_, dataTable) => true;

                // Act, Assert
                _northStarPrivateObject.Invoke(LogNorthstarRelationalMessageMethod, _clientMethodsData, _dtGroup, command);
            }
        }

        private void InitializePrivateObject()
        {
            _northStarPrivateObject = new PrivateObject(Northstar, new PrivateType(typeof(Northstar)));
        }
    }
}
