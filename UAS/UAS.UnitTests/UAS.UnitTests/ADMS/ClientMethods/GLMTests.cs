using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using ADMS.ClientMethods;
using ADMS.ClientMethods.Fakes;
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
    public class GLMTests
    {
        private readonly GLM Glm = new GLM();
        private PrivateObject _glmPrivateObject;
        private BusinessClientMethods _clientMethodsData;
        private DataTable _dtNyNow;

        private const string InsertGlmRelationalDataMethod = "InsertGlmRelationalData";

        [Test]
        public void CreateGLMRelationalFiles_DirectoryNotExist_NoGetFilesCalled()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new GLM();
                var client = new KMPlatform.Entity.Client();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = info =>
                {
                    getFilesCalled = true;
                    return null;
                };

                // Act
                testObject.CreateGLMRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeFalse();
            }
        }

        [Test]
        public void CreateGLMRelationalFiles_DirectoryExists_NoFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new FileInfo[] { };
                };

                // Act
                testObject.CreateGLMRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateGLMRelationalFiles_DirectoryExists_PartialFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("icff365 drupal leads march 25.csv"),
                        new FileInfo("nynow365 drupoal leads march 25.csv"),
                    };
                };

                // Act
                testObject.CreateGLMRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateGLMRelationalFiles_DirectoryExists_AllFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;
                var tempTablesCleared = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("icff365 drupal leads march 25.csv"),
                        new FileInfo("nynow365 drupoal leads march 25.csv"),
                        new FileInfo("sigmix365 drupal leads march 25.csv"),
                    };
                };

                ShimClientMethods.AllInstances.GLM_DropTempCMSTables = methods =>
                {
                    tempTablesCleared = true;
                    return true;
                };

                ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                    (_, __, ___, _4, _5, _6) => { };

                // Act
                testObject.CreateGLMRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
                tempTablesCleared.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateGLMRelationalFiles_AllFilesFound_ShouldReturnWithoutErrors()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;
                var tempTablesCleared = false;
                var wasException = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.GLM_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("icff365 drupal leads march 25.csv"),
                        new FileInfo("nynow365 drupoal leads march 25.csv"),
                        new FileInfo("sigmix365 drupal leads march 25.csv"),
                    };
                };

                ShimClientMethods.AllInstances.GLM_DropTempCMSTables = methods =>
                {
                    tempTablesCleared = true;
                    return true;
                };

                ShimClientMethods.AllInstances.GLM_Relational_Update = methods => true;

                ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                    (_, __, ___, _4, _5, _6) => { wasException = true; };

                ShimClientMethodHelpers.ProcessImportVesselDataFileInfoFileConfiguration =
                    (info, configuration) => null;

                ShimGLM.AllInstances.InsertGlmRelationalDataClientMethodsDataTable = (glm, methods, arg3) => { };

                // Act
                testObject.CreateGLMRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
                tempTablesCleared.ShouldBeTrue();
                wasException.ShouldBeFalse();
            }
        }

        [Test]
        public void SwipeDataFile_DirectoryNotExist_NoGetFilesCalled()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new GLM();
                var client = new KMPlatform.Entity.Client();
                var getFilesCalled = false;                

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = info =>
                {
                    getFilesCalled = true;
                    return null;
                };

                // Act
                testObject.SwipeDataFile(client, null, null);

                // Assert
                getFilesCalled.ShouldBeFalse();
            }
        }

        [Test]
        public void SwipeDataFile_DirectoryExists_NoFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new FileInfo[] { };
                };

                // Act
                testObject.SwipeDataFile(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void SwipeDataFile_DirectoryExists_PartialFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;
                
                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("Pier92RawET version 2.xlsx"),
                        new FileInfo("nynow365 drupoal leads march 25.csv"),
                    };
                };

                // Act
                testObject.SwipeDataFile(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void SwipeDataFile_DirectoryExists_AllFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new GLM();
                var getFilesCalled = false;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("Pier92RawET version 2.xlsx"),
                        new FileInfo("icff365 drupal leads march 25.csv"),
                        new FileInfo("nynow365 drupoal leads march 25.csv"),
                        new FileInfo("sigmix365 drupal leads march 25.csv"),
                    };
                };
                ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                    (_, __, ___, _4, _5, _6) => { };

                ShimFtpFunctions.ConstructorStringStringString = (_, __, ___, ____) => { };
                KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.UploadStringStringBoolean = (_, __, ___, ____) => true;
                ShimClientFTP.AllInstances.SelectClientInt32 = (ftp, i) => new List<ClientFTP>()
                {
                    new ClientFTP()
                };

                // Act
                testObject.SwipeDataFile(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void InsertGlmRelationalData_WhenClientMethodsDataIsNull_ThrowsException()
        {
            // Arrange
            InitializePrivateObject();
            _clientMethodsData = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _glmPrivateObject.Invoke(InsertGlmRelationalDataMethod, _clientMethodsData, _dtNyNow));
        }

        [Test]
        public void InsertGlmRelationalData_WhenDataTableNyNowIsNull_ThrowsException()
        {
            // Arrange
            InitializePrivateObject();
            _clientMethodsData = new BusinessClientMethods();
            _dtNyNow = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _glmPrivateObject.Invoke(InsertGlmRelationalDataMethod, _clientMethodsData, _dtNyNow));
        }

        [Test]
        public void InsertGlmRelationalData_WhenClientMethodsDataAndDataTableNyNowAreNotNull_ShouldInsertData()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                InitializePrivateObject();
                _clientMethodsData = new BusinessClientMethods();
                _dtNyNow = new DataTable();

                ShimClientMethods.AllInstances.GLM_Relational_InsertDataDataTable = (_, dataTable) => true;

                // Act, Assert
                _glmPrivateObject.Invoke(InsertGlmRelationalDataMethod, _clientMethodsData, _dtNyNow);
            }
        }

        private void InitializePrivateObject()
        {
            _glmPrivateObject = new PrivateObject(Glm, new PrivateType(typeof(GLM)));
        }
    }
}
