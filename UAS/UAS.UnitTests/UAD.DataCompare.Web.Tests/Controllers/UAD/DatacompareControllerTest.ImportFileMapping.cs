using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using NUnit.Framework;
using UAD.DataCompare.Web.Models;
using Shouldly;
using Moq;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.Object.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using UAD.DataCompare.Web.Controllers.UAD.Fakes;

namespace UAD.DataCompare.Web.Tests.Controllers.UAD
{
    public partial class DatacompareControllerTest
    {
        private const string SampleDataColumn = "SampleColumn";
        private const string SampleColumnName = "SampleColumnName";
        private const string ColumnTypeIgnore = "Ignore";
        private const string KeySteps = "steps";
        private const string KeyExtention = "Extention";
        private const string MapcolumnsStep = "Mapcolumns";
        private const string SavemappingStep = "Savemapping";
        private const string SaveAndImportStep = "SaveAndImport";
        private const string SomeOtherStep = "Other";
        private const string KeyPath = "path";
        private const string FileIsMissingError = "FileIsMissing";
        private const string NullDelOrQuoteError = "NullDelOrQuote";
        private const string NullDataError = "NullData";
        private const string SaveAsNameAlreadyExistError = "SaveAsNameAlreadyExist";
        private const string SFE_NoFileExtentionError = "SFE_NoFileExtention";
        private const string SFE_SaveFailedError = "SFE_SaveFailed";
        private const string FileIsNotPresentError = "FileIsNotPresent";
        private const string MoreThanOneFTPSettingError = "MoreThanOneFTPSetting";
        private const string ErrorValidationError = "ErrorValidation";

        [Test]
        public void ImportFileMapping_WhenFileDataIsNull_AddsModelStateError()
        {
            // Arrange
            var fileDetails = GetFileDetails();
            var formCollection = new FormCollection(GetCollection());
            SetFakesForViewComparision();
            SetupControllerContext();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(FileIsMissingError).ShouldBeTrue(),
                () => _controller.ModelState[FileIsMissingError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[FileIsMissingError].Errors[0].ErrorMessage.ShouldContain("Please select file to map."));
        }

        [Test]
        public void ImportFileMapping_WhenFileDataIsNotNullButMissingDelimiter_AddsModelStateError()
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            var formCollection = new FormCollection(GetCollection());
            SetFakesForViewComparision();
            SetupControllerContext();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(NullDelOrQuoteError).ShouldBeTrue(),
                () => _controller.ModelState[NullDelOrQuoteError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[NullDelOrQuoteError].Errors[0].
                            ErrorMessage.ShouldContain(@"Missing File Info: File Delimiter and/or File contains double quotation marks"));
        }
        [Test]
        public void ImportFileMapping_WhenFileNameIsEmpty_AddsModelStateError()
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.FileName = string.Empty;
            fileDetails.DataFile = GetPostedFileMock().Object;
            var formCollection = new FormCollection(GetCollection());
            SetFakesForViewComparision();
            SetupControllerContext();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(NullDataError).ShouldBeTrue(),
                () => _controller.ModelState[NullDataError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[NullDataError].Errors[0].
                            ErrorMessage.ShouldContain(@"Data is missing:Please make sure client was selected and/or file was selected."));
        }

        [Test]
        public void ImportFileMapping_WhenFileNameAlreadyExists_AddsModelStateError()
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            var formCollection = new FormCollection(GetCollection());
            SetFakesForViewComparision(fileName: SampleFileName);
            SetupControllerContext();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(SaveAsNameAlreadyExistError).ShouldBeTrue(),
                () => _controller.ModelState[SaveAsNameAlreadyExistError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[SaveAsNameAlreadyExistError].Errors[0].
                            ErrorMessage.ShouldContain(@"File Previously Mapped"));
        }

        [Test]
        public void ImportFileMapping_WhenNewFileName_AddsColumnMappingAndViewBagProperty()
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            var formCollection = new FormCollection(GetCollection());
            SetFakesForViewComparision();
            SetupControllerContext();
            SetFakesForFileMapping();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
               () => _controller.Session[KeyProfileColumnList].ShouldNotBeNull(),
               () => 
                   {
                       var isSuccess = (bool)_controller.ViewBag.MappingSuccess;
                       isSuccess.ShouldBeTrue();
                   },
               () => fileDetails.ColumnMapping.Count.ShouldBe(1),
               () => fileDetails.ColumnMapping[0].PreviewDataColumn.ShouldBe("1"),
               () => fileDetails.ColumnMapping[0].MappedColumn.ShouldBe(ColumnTypeIgnore),
               () => fileDetails.ColumnMapping[0].SourceColumn.ShouldBe(SampleDataColumn.ToLower()),
               () => fileDetails.ColumnMapping[0].ProfileColumnList.Count.ShouldBe(4),
               () => fileDetails.ColumnMapping[0].ProfileColumnList.ShouldContain(x => x.Value == SampleColumnName),
               () => fileDetails.ColumnMapping[0].PreviewDataColumn.ShouldBe("1"));
        }

        [Test]
        [TestCase(SavemappingStep)]
        [TestCase(SaveAndImportStep)]
        public void ImportFileMapping_WhenStepIsSaveMappingAndNoFileExtension_AddsModelStateError(string stepType)
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            var collection = GetCollection();
            collection.Set(KeySteps, stepType);
            var formCollection = new FormCollection(collection);
            SetFakesForViewComparision();
            SetupControllerContext();
            SetFakesForFileMapping();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(2),
                () => _controller.ModelState.ContainsKey(SFE_NoFileExtentionError).ShouldBeTrue(),
                () => _controller.ModelState[SFE_NoFileExtentionError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[SFE_NoFileExtentionError].Errors[0].
                            ErrorMessage.ShouldContain(@"File extetion can not be detected."),
                () => _controller.ModelState.ContainsKey(SFE_SaveFailedError).ShouldBeTrue(),
                () => _controller.ModelState[SFE_SaveFailedError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[SFE_SaveFailedError].Errors[0].
                            ErrorMessage.ShouldContain(@"File save failed."));
        }

        [Test]
        [TestCase(KeyExtention)]
        [TestCase(KeyPath)]
        public void ImportFileMapping_WhenStepIsSaveMappingWithFileExtension_SavesFileAndRedirects(string sessionKey)
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            fileDetails.ColumnMapping[0].MappedColumn = SampleSourceColumn;
            var collection = GetCollection();
            collection.Set(KeySteps, SavemappingStep);
            var formCollection = new FormCollection(collection);
            SetFakesForViewComparision();
            SetFakesForSaveSourceFileMethod();
            SetFakesForSaveFieldMappingNew();
            session.Setup(x => x[sessionKey]).Returns(SampleFileExtension);
            SetupControllerContext();
            SetFakesForFileMapping();

            // Act
            var actionResult = _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            actionResult.ShouldSatisfyAllConditions(
                () => 
                {
                    var isFieldSaveSuccess = (bool)_controller.ViewBag.FieldSaveSuccess;
                    isFieldSaveSuccess.ShouldBeTrue();
                },
                () => 
                {
                    var redirectResult = actionResult.ShouldBeOfType<RedirectToRouteResult>();
                    redirectResult.RouteValues.Values.OfType<string>().ShouldBe(new [] {"Index", "Datacompare"});
                });
        }

        [Test]
        [TestCase(KeyPath)]
        public void ImportFileMapping_WhenStepIsSaveAndImportWithFileExtension_SavesFileAndRedirects(string sessionKey)
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            fileDetails.ColumnMapping[0].MappedColumn = SampleSourceColumn;
            var collection = GetCollection();
            collection.Set(KeySteps, SaveAndImportStep);
            var formCollection = new FormCollection(collection);
            SetFakesForViewComparision();
            SetFakesForSaveSourceFileMethod();
            SetFakesForSaveFieldMappingNew();
            session.Setup(x => x[sessionKey]).Returns(SampleFileExtension);
            SetupControllerContext();
            SetFakesForFileMapping();
            ShimDatacompareController.AllInstances.UploadFileToFtpFileInfo = (_, __) => true;

            // Act
            var actionResult = _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            actionResult.ShouldSatisfyAllConditions(
                () =>
                {
                    var isFieldSaveSuccess = (bool)_controller.ViewBag.ImportSuccess;
                    isFieldSaveSuccess.ShouldBeTrue();
                },
                () =>
                {
                    var redirectResult = actionResult.ShouldBeOfType<RedirectToRouteResult>();
                    redirectResult.RouteValues.Values.OfType<string>().ShouldBe(new[] { "Index", "Datacompare" });
                });
        }

        [Test]
        [TestCase(KeyExtention)]
        public void ImportFileMapping_WhenStepIsSaveAndImportWithFileExtension_AddsModelStateError(string sessionKey)
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            fileDetails.ColumnMapping[0].MappedColumn = SampleSourceColumn;
            var collection = GetCollection();
            collection.Set(KeySteps, SaveAndImportStep);
            var formCollection = new FormCollection(collection);
            SetFakesForViewComparision();
            SetFakesForSaveSourceFileMethod();
            SetFakesForSaveFieldMappingNew();
            session.Setup(x => x[sessionKey]).Returns(SampleFileExtension);
            SetupControllerContext();
            SetFakesForFileMapping();
            ShimDatacompareController.AllInstances.UploadFileToFtpFileInfo = (_, __) => true;

            // Act
            var actionResult = _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            actionResult.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(FileIsNotPresentError).ShouldBeTrue(),
                () => _controller.ModelState[FileIsNotPresentError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[FileIsNotPresentError].Errors[0].
                            ErrorMessage.ShouldContain(@"Please select file for updload."));
        }

        [Test]
        [TestCase(KeyPath)]
        public void ImportFileMapping_WhenStepIsSaveAndImportAndUploadToFTPFalse_AddsModelStateError(string sessionKey)
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            fileDetails.ColumnMapping[0].MappedColumn = SampleSourceColumn;
            var collection = GetCollection();
            collection.Set(KeySteps, SaveAndImportStep);
            var formCollection = new FormCollection(collection);
            SetFakesForViewComparision();
            SetFakesForSaveSourceFileMethod();
            SetFakesForSaveFieldMappingNew();
            session.Setup(x => x[sessionKey]).Returns(SampleFileExtension);
            SetupControllerContext();
            SetFakesForFileMapping();
            ShimDatacompareController.AllInstances.UploadFileToFtpFileInfo = (_, __) => false;

            // Act
            var actionResult = _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            actionResult.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState[MoreThanOneFTPSettingError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(MoreThanOneFTPSettingError).ShouldBeTrue(),
                () => _controller.ModelState[MoreThanOneFTPSettingError].Errors[0].
                            ErrorMessage.ShouldContain(@"More Than One FTP Setting for Client"));
        }

        [Test]
        public void ImportFileMapping_WhenStepTypeIsOther_AddsModelStateError()
        {
            // Arrange
            var fileDetails = GetFileDetails();
            fileDetails.DataFile = GetPostedFileMock().Object;
            fileDetails.HasQuotation = YesString;
            fileDetails.Delimiter = SampleDelimiter;
            var collection = GetCollection();
            collection.Set(KeySteps, SomeOtherStep);
            var formCollection = new FormCollection(collection);
            SetFakesForViewComparision();
            SetupControllerContext();

            // Act
            _controller.ImportFileMapping(fileDetails, formCollection);

            // Assert
            _controller.ShouldSatisfyAllConditions(
                () => _controller.ModelState.Keys.Count.ShouldBe(1),
                () => _controller.ModelState.ContainsKey(ErrorValidationError).ShouldBeTrue(),
                () => _controller.ModelState[ErrorValidationError].Errors.Count.ShouldBe(1),
                () => _controller.ModelState[ErrorValidationError].Errors[0].
                            ErrorMessage.ShouldContain(@"Please check all the details are entered correctly"));
        }

        private Mock<HttpPostedFileBase> GetPostedFileMock()
        {
            var postedFileMock = new Mock<HttpPostedFileBase>();
            postedFileMock.Setup(x => x.FileName).Returns($"{SampleFileName}.txt");
            postedFileMock.Setup(x => x.InputStream).Returns(Stream.Null);
            return postedFileMock;
        }

        private NameValueCollection GetCollection()
        {
            var collection = new NameValueCollection();
            collection.Add(KeySteps, MapcolumnsStep);
            return collection;
        }

        private FileDetails GetFileDetails()
        {
            return new FileDetails
            {
                SourceFileID = 1,
                FileName = SampleFileName,
                ColumnMapping = new List<ColumnMap>
                {
                    new ColumnMap{ FieldMapID = 1, SourceFileID = 1 }
                },
                IsImportBillable = YesString,
            };
        }

        private void SetFakesForFileMapping()
        {
            ShimFileWorker.AllInstances.GetDuplicateColumnsFileInfoFileConfiguration = (_, __, ___) => new List<string>
            {
                SampleDataColumn
            };
            ShimFileWorker.AllInstances.GetFileHeadersFileInfoFileConfigurationBoolean = (_, __, ___, ____) => new StringDictionary
            {
                [SampleDataColumn] = "1"
            };
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (_, __, ___) => GetDataTable();
            ShimDBWorker.GetPubIDAndCodesByClientClient = (_) => new Dictionary<int, string>
            {
                [1] = SamplePubName
            };
            ShimFileMappingColumn.AllInstances.SelectClientConnections = (_, __) => new List<FrameworkUAD.Object.FileMappingColumn>
            {
                new FrameworkUAD.Object.FileMappingColumn{ ClientId = 1, ColumnName = SampleColumnName }
            };
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(SampleDataColumn);
            var row = dataTable.NewRow();
            row[SampleDataColumn] = "1";
            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}
