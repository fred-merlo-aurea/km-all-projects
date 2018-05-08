using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.IO.Fakes;
using System.Web;
using System.Web.Fakes;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using System.Configuration.Fakes;
using NUnit.Framework;
using ecn.communicator.mvc.Controllers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ecn.communicator.mvc.Infrastructure.Fakes;
using Shouldly;
using ecn.communicator.mvc.Controllers.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using MicrosoftUnitTesting =  Microsoft.VisualStudio.TestTools.UnitTesting;
using PlatformFakes =  KM.Platform.Fakes;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        private const string ResultStringDublicatedFields = "You have selected duplicate field names";
        private const string ResultStringEmailRequired = "Email Address is required to import data.";
        private const string ResultStringCheckSheetName = "Please check the sheet name.";
        private const string ResultStringNoRowsUpdated = "0 rows updated/inserted";
        private const string ResultStringException= "An error occurred and was logged.";
        private const string OleDbExceptionMessage = "'$' is not a valid name.  " +
            "Make sure that it does not include invalid characters or punctuation and that it is not too long";
        private const string DirectoryName = "dir.txt";
        private const string FileName = "dir.txt";
        private const string StringTen = "10";
        private const string Text = "txt";
        private const string SelectedGroupId = "selectedGroupID";
        private const string Type = "I";
        private const string Guid = "0f8fad5b-d9cb-469f-a165-70867728950e";
        private const int FileLength = 1000;
        private DataTable _dataTable;

        [TearDown]
        public void TearDown()
        {
            if(_dataTable != null)
            {
                _dataTable.Dispose();
            }
        }

        [Test]
        public void ImportDataAction_ValidData_ReturnJson()
        {
            InitilizeImportDataActionTests();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { "EmailAddress", "ignore", "user_", "data" });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringSuccessfullyAdded));
        }

        [Test]
        public void ImportDataAction_DublicateFields_ReturnJson()
        {
            InitilizeImportDataActionTests();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { "EmailAddress", "EmailAddress", "EmailAddress", "EmailAddress" });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringDublicatedFields));
        }

        [Test]
        public void ImportDataAction_NoField_ReturnJson()
        {
            InitilizeImportDataActionTests();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { "ignore", "ignore", "ignore", "ignore" });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringEmailRequired));
        }

        [Test]
        public void ImportDataAction_NoEmailAddress_ReturnJson()
        {
            InitilizeImportDataActionTests();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { "ignore", "ignore", "user_", "data" });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringEmailRequired));
        }

        [Test]
        public void ImportDataAction_ExceptionOnFields_ReturnJson()
        {
            InitilizeImportDataActionTests();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringEmailRequired));
        }

        [Test]
        public void ImportDataAction_NoRecordUpdate_ReturnJson()
        {
            InitilizeImportDataActionTests();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => new DataTable();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { "EmailAddress", "ignore", "user_", "data" });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringNoRowsUpdated));
        }

        [Test]
        public void ImportDataAction_ExceptionOnGroupDataFields_ReturnJson()
        {
            InitilizeImportDataActionTests();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => throw new Exception();
            var controller = new GroupController();
            var importDataOptions = CreateImportDataOptions(new List<string> { "EmailAddress", "ignore", "user_", "data" });
            var result = controller.ImportDataAction(importDataOptions) as JsonResult;

            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 2);
            Assert.AreEqual(jsonResult[0], HttpStatusInternalServerError);
            Assert.IsTrue(jsonResult[1].Contains(ResultStringException));
        }

        [Test]
        public void ImportManager_ForAccessedUser_ReturnActionResult()
        {
            // Arrange
            InitializeImport();
            var controller = new GroupController();

            // Act
            var result = controller.ImportManager() as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void AddFiles_ForRequestFiles_ReturnActionResult()
        {
            // Arrange
            InitializeAddFiles();
            var controller = new GroupController();

            // Act
            var result = controller.AddFiles() as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void UploadFilesToServer_ForEmptyFiles_ReturnActionResult()
        {
            // Arrange
            var controller = new GroupController();
            InitializeUpload();

            // Act
            var result = controller.UploadFilesToServer() as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void GetFiles_ForDataSource_ReturnActionResult()
        {
            // Arrange
            InitializeImport();
            var controller = new GroupController();

            // Act
            var result = controller.GetFiles() as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void DeleteDataFile_ForInvalidFileInfo_ReturnActionResult()
        {
            // Arrange
            InitializeDeleteData();
            var controller = new GroupController();

            // Act
            var result = controller.DeleteDataFile(DirectoryName) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void ImportDataFromFile_ForAccessedUser_ReturnActionRessult()
        {
            // Arrange
            InitializeImportData();
            var controller = new GroupController();

            // Act
            var result = controller.ImportDataFromFile(string.Empty, 
                string.Empty, string.Empty, StringTen, Text, 
                string.Empty, string.Empty, string.Empty) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void CheckImportDataFromFile_ForEmptyFile_ReturnActionResult()
        {
            // Arrange
            InitializeImportData();
            var controller = new GroupController();

            // Act
            var result = controller.CheckImportDataFromFile(string.Empty,
                string.Empty, string.Empty, StringTen, Text,
                string.Empty, string.Empty, string.Empty) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        [Test]
        public void DownloadImportedEmails_ForDownloadTypeXls_ReturnNullFileContentResult()
        {
            // Arrange
            var controller = new GroupController();
            InitializeDownload();

            // Act
            var result = controller.DownloadImportedEmails(Type, FileName, Guid);

            // Assert
            result.ShouldBeNull();
        }

        private void InitializeDownload()
        {
            CreateCurrentUser();
            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { "KMCommon_Application", "1" },
                    { "Images_VirtualPath", "path" }
                };
            var httpBase = new Moq.Mock<HttpServerUtilityBase>();
            httpBase.Setup(x => x.MapPath(Moq.It.IsAny<string>())).Returns(DirectoryName);
            ShimController.AllInstances.ServerGet = (x) => httpBase.Object;
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new DirectoryInfo(DirectoryName);
            ShimFile.ExistsString = (x) => true;
            _dataTable = new DataTable()
            {
                Columns = { "Column1", "Colummn2", "Colummn3", "Colummn4" },
                Rows = { { "Data1", "Data2", "Data3", "Data4" } }
            };
            ShimEmailGroup.ExportFromImportEmailsUserStringString = (x, y, z) => _dataTable;
            var response = new Moq.Mock<HttpResponseBase>();
            ShimController.AllInstances.ResponseGet = (x) => response.Object;
        }

        private void InitializeImportData()
        {
            CreateCurrentUser();
            PlatformFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            ShimGroup.GetByGroupIDInt32User = (x, y) => new Group() { GroupName = DirectoryName };
            _dataTable = new DataTable()
            {
                Columns = { "Column1", "Colummn2", "Colummn3", "Colummn4" },
                Rows = { { "Data1", "Data2", "Data3", "Data4" } }
            };
            ShimGroupController.AllInstances.GetDataTableByFileTypeImportDataOptionsInt32 =
                (x, y, z) => _dataTable;
            var httpStateBase = new Moq.Mock<HttpSessionStateBase>();
            httpStateBase.Setup(x => x.Add(SelectedGroupId, DirectoryName));
            ShimController.AllInstances.SessionGet = (x) => httpStateBase.Object;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = 
                (x, y, z) => new List<GroupDataFields> { new GroupDataFields() { ShortName = DirectoryName } };
        }

        private static void InitializeDeleteData()
        {
            InitializeUpload();
            InitializeImport();
            ShimControllerBase.AllInstances.ControllerContextGet = (x) => new ControllerContext();
            ShimHtmlHelperMethods.RenderViewToStringControllerContextStringObject = (x, y, z) => DirectoryName;
            var httpBase = new Moq.Mock<HttpResponseBase>();
            ShimController.AllInstances.ResponseGet = (x) => httpBase.Object; 
        }

        private static void InitializeUpload()
        {
            CreateCurrentUser();
            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { "KMCommon_Application", "1" },
                    { "Images_VirtualPath", "path" }
                };
            var httpBase = new Moq.Mock<HttpServerUtilityBase>();
            var httpStateBase = new Moq.Mock<HttpSessionStateBase>();
            ShimController.AllInstances.ServerGet = (x) => httpBase.Object;
            ShimController.AllInstances.SessionGet = (x) => httpStateBase.Object;
        }

        private static void InitializeAddFiles()
        {
            var httpBase = new Moq.Mock<HttpRequestBase>();
            var httpFileBase = new Moq.Mock<HttpPostedFileBase>();
            var httpCollectionBase = new Moq.Mock<HttpFileCollectionBase>();
            httpCollectionBase.Setup(x => x.Get(0)).Returns(httpFileBase.Object);
            httpCollectionBase.Setup(x => x.Count).Returns(1);
            var httpStateBase = new Moq.Mock<HttpSessionStateBase>();
            httpFileBase.Setup(x => x.ContentLength).Returns(1);
            httpBase.Setup(x => x.Files).Returns(httpCollectionBase.Object);
            ShimController.AllInstances.RequestGet = (x) => httpBase.Object;
            ShimHttpFileCollectionBase.AllInstances.CountGet = (x) => 1;
            ShimController.AllInstances.SessionGet = (x) => httpStateBase.Object;
        }

        private static void InitializeImport()
        {
            CreateCurrentUser();
            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { "KMCommon_Application", "1" },
                    { "Images_VirtualPath", "path" }
                };
            PlatformFakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            ShimHttpServerUtilityBase.AllInstances.MapPathString = (x, y) => DirectoryName;
            ShimDirectory.ExistsString = (x) => false;
            ShimDirectory.CreateDirectoryString = (x) => new DirectoryInfo(DirectoryName);
            ShimDirectory.GetFilesStringString = (x, y) => new string[] { DirectoryName };
            ShimFileInfo.AllInstances.NameGet = (x) => DirectoryName;
            ShimFileInfo.AllInstances.LengthGet = (x) => FileLength;
            ShimFileSystemInfo.AllInstances.LastWriteTimeGet = (x) => new DateTime();
            var httpBase = new Moq.Mock<HttpServerUtilityBase>();
            var httpStateBase = new Moq.Mock<HttpSessionStateBase>();
            ShimController.AllInstances.ServerGet = (x) => httpBase.Object;
            ShimController.AllInstances.SessionGet = (x) => httpStateBase.Object;
        }

        private void InitilizeImportDataActionTests()
        {
            ShimConfigurationManager.AppSettingsGet =
                () => new System.Collections.Specialized.NameValueCollection
                {
                    { "KMCommon_Application", "1" },
                    { "Images_VirtualPath", "" }
                };
            ShimGroupController.AllInstances.getPhysicalPath = (p) => String.Empty;
            _dataTable = new DataTable()
            {
                Columns = { "Column1", "Colummn2", "Colummn3", "Colummn4" },
                Rows = { { "Data1", "Data2", "Data3", "Data4" } }
            };
            ShimFileImporter.GetDataTableByFileTypeStringStringStringStringInt32Int32String =
                (x1, x2, x3, x4, x5, x6, x7) => _dataTable;
            ShimGroupController.AllInstances.CreateNoteStringString = (x1, x2, x3) => string.Empty;
            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, name, appId, note, gd, ec) => { };

            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (x1, x2, x3) => new List<GroupDataFields>
            {
                new GroupDataFields { }
            };

            ShimHttpContext.CurrentGet = () => new ShimHttpContext { };
            ShimGroup.GetByCustomerIDInt32UserString = (x1, x2, x3) => new List<Group>
            {
                new Group
                {
                    GroupID = SampleDataTableGroupId
                }
            };

            ShimGroup.GetMasterSuppressionGroupInt32User = (x1, x2) => new Group {  };
            ShimGroup.GetByGroupIDInt32User = (x1, x2) => new Group
            {
                GroupID = SampleDataTableGroupId
            };

            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x1, x2, x3) => new EmailGroup { };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => new DataTable
                {
                    Columns = { SampleColumAction, SampleColumCounts },
                    Rows =
                    {
                        { ActionTypeT, ActionCount2 },
                        { ActionTypeT, ActionCount3 },
                        { ActionTypeI, ActionCount3 },
                        { ActionTypeU, ActionCount3 },
                        { ActionTypeD, ActionCount3 },
                        { ActionTypeS, ActionCount3 },
                        { ActionTypeM, ActionCount3 }
                    }
                };

            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
        }

        private ecn.communicator.mvc.Models.ImportDataOptions CreateImportDataOptions(List<string> dropdownValues)
        {
            return new ecn.communicator.mvc.Models.ImportDataOptions
            {
                fileType = string.Empty,
                sheetName = string.Empty,
                dl = string.Empty,
                dropDownValues = dropdownValues,
                gid = "1"
            };

        }
    }
}
