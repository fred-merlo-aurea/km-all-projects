using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ADMS.ClientMethods;
using Core.ADMS.Events;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KM.Common.Import;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.ClientMethods.Common;
using ClientEntity = KMPlatform.Entity.Client;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;
using ImportVesselObject = FrameworkUAD.Object.ImportVessel;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ClientMethodHelpersTest : Fakes
    {
        private Dictionary<int, DataRow> _newRows;
        private Dictionary<int, DataRow> _resultRows;
        private DataTable _workDataTable;
        private DataRow _originalRows;
        private List<string> _pubCodes;

        private string _columnValue = string.Empty;
        private string _value = string.Empty;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            _workDataTable = new DataTable();
        }

        [Test]
        public void AddRow_WhenNewRowsIsNull_ThrowsException()
        {
            // Arrange
            _newRows = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
                ClientMethodHelpers.AddRow(PubCodeUpperKey, 0, _newRows, _workDataTable, _originalRows));
        }

        [Test]
        public void AddRow_WhenDataTableIsNull_ThrowsException()
        {
            // Arrange
            _newRows = new Dictionary<int, DataRow>();
            _workDataTable = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => 
                ClientMethodHelpers.AddRow(PubCodeUpperKey, _newRows.Count + 1, _newRows, _workDataTable, _originalRows));
        }

        [Test]
        public void AddRow_WhenOriginalRowsIsNull_ThrowsException()
        {
            // Arrange
            _newRows = new Dictionary<int, DataRow>();
            _workDataTable = new DataTable();
            _originalRows = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ClientMethodHelpers.AddRow(PubCodeUpperKey, _newRows.Count + 1, _newRows, _workDataTable, _originalRows));
        }

        [Test]
        public void AddRow_ShoudCreateNewDataRow_AddsNewRowInDictionary()
        {
            // Arrange
            CreateSampleDataTable();
            CreateDictionaryForNewRows();

            var position = _newRows.Count + 1;
            _originalRows = _workDataTable.Rows[0];
            CreateDataTableResult(position);

            // Act
            ClientMethodHelpers.AddRow(PubCodeUpperKey, position, _newRows, _workDataTable, _originalRows);

            // Assert
            _newRows.ShouldSatisfyAllConditions(
                () => _newRows.Count.ShouldBe(position),
                () => _newRows[1].ItemArray.ShouldBe(_workDataTable.Rows[0].ItemArray),
                () => _newRows[2].ItemArray.ShouldBe(_resultRows[2].ItemArray));
        }

        [Test]
        public void AddPubCode_WhenPubCodesIsNull_ThrowsException()
        {
            // Arrange
            _columnValue = string.Empty;
            _value = string.Empty;
            _pubCodes = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.AddPubCode(_columnValue, _pubCodes, _value));
        }

        [Test]
        public void AddPubCode_WhenColumnValueIsYes_AddsNewValueInCollection()
        {
            // Arrange
            _columnValue = YesCode;
            _value = SampleUsername;
            _pubCodes = new List<string>();

            // Act
            ClientMethodHelpers.AddPubCode(_columnValue, _pubCodes, _value);

            // Assert
            _pubCodes.ShouldSatisfyAllConditions( 
                () => _pubCodes.Count.ShouldBe(1), 
                () => _pubCodes[0].ShouldBe(_value));
        }

        [Test]
        public void AddPubCode_WhenColumnValueIsNotYes_WillNotAddElementInCollection()
        {
            // Arrange
            _columnValue = NoCode;
            _value = SampleUsername;
            _pubCodes = new List<string>();

            // Act
            ClientMethodHelpers.AddPubCode(_columnValue, _pubCodes, _value);

            // Assert
            _pubCodes.ShouldBeEmpty();
        }

        [Test]
        [TestCase(null, "", "comma", ".csv", true)]
        [TestCase(null, "", "comma", ".csv", false)]
        [TestCase(1, "Sample 1", "comma", ".csv", false)]
        [TestCase(null, "", "tab", ".txt", true)]
        [TestCase(null, "", "tab", ".txt", false)]
        [TestCase(2, "Sample2", "tab", ".txt", false)]
        public void CreateFileConfiguration_ShouldCreateNewFileConfiguration_ReturnsFileConfiguration(
            int? columnCount,
            string headers,
            string fileDelimiter,
            string fileExtension,
            bool isQuoteEncapsulated)
        {
            // Arrange
            var columnCountResult = columnCount ?? 0;
            var columnHeadersResult = string.IsNullOrWhiteSpace(headers) ? null : headers;

            // Act
            var result = ClientMethodHelpers.CreateFileConfiguration(columnCount, headers, fileDelimiter, fileExtension, isQuoteEncapsulated);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ColumnCount.ShouldBe(columnCountResult),
                () => result.ColumnHeaders.ShouldBe(columnHeadersResult),
                () => result.FileColumnDelimiter.ShouldBe(fileDelimiter),
                () => result.FileExtension.ShouldBe(fileExtension),
                () => result.IsQuoteEncapsulated.ShouldBe(isQuoteEncapsulated));
        }

        [Test]
        public void ProcessImportVesselData_WhenFileInfoIsNull_ThrowsException()
        {
            // Arrange
            FileInfo = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.ProcessImportVesselData(FileInfo, FileConfigurationAccess));
        }

        [Test]
        public void ProcessImportVesselData_WhenFileConfigurationAccessIsNull_ThrowsException()
        {
            // Arrange
            SetupFakes();
            FileInfo = CreateFileInfo($"{DummyName}{Consts.CsvExtension}");
            FileConfigurationAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.ProcessImportVesselData(FileInfo, FileConfigurationAccess));
        }

        [Test]
        public void ProcessImportVesselData_ShouldProcessRows_ReturnsDataTable()
        {
            // Arrange
            DtAccess = new DataTable();
            SetupFakes();
            FileInfo = CreateFileInfo($"{DummyName}{Consts.CsvExtension}");
            FileConfigurationAccess = new FileConfiguration();

            CreateDataTableAccessForBmAutoGenAlgorithms();

            ShimForFileRows(DtAccess.Rows.Count);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            // Act
            var result = ClientMethodHelpers.ProcessImportVesselData(FileInfo, FileConfigurationAccess);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<DataTable>(),
                () => result.Rows.Count.ShouldBe(DtAccess.Rows.Count));
        }

        [Test]
        public void CreateEmailHeader_WhenClientEntityIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.CreateEmailHeader(ClientEntity, FileConfigurationAccess, DtAccess));
        }

        [Test]
        public void CreateEmailHeader_WhenFileConfigurationAccessIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved();
            FileConfigurationAccess = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.CreateEmailHeader(ClientEntity, FileConfigurationAccess, DtAccess));
        }

        [Test]
        public void CreateEmailHeader_WhenDtAccessIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();
            DtAccess = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.CreateEmailHeader(ClientEntity, FileConfigurationAccess, DtAccess));
        }

        [Test]
        public void CreateEmailHeader_ShouldAppendHeaders_SendEmail()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            FileConfigurationAccess = new FileConfiguration();
            CreateDataTableAccessForDemo();
            var sbResult = SetEmailHeaders(DtAccess);
            
            // Act
            var result = ClientMethodHelpers.CreateEmailHeader(ClientEntity, FileConfigurationAccess, DtAccess);

            // Arrange
            result.ToString().ShouldBe(sbResult.ToString());
        }

        [Test]
        public void AppendImportErrors_WhenFileConfigurationAccessIsNull_ThrowsException()
        {
            // Arrange
            FileConfigurationAccess = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.AppendImportErrors(FileConfigurationAccess, ImportErrors, SbDetail));
        }

        [Test]
        public void AppendImportErrors_WhenImportErrorsIsNull_ThrowsException()
        {
            // Arrange
            FileConfigurationAccess = new FileConfiguration();
            ImportErrors = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.AppendImportErrors(FileConfigurationAccess, ImportErrors, SbDetail));
        }

        [Test]
        public void AppendImportErrors_WhenSbDetailIsNull_ThrowsException()
        {
            // Arrange
            FileConfigurationAccess = new FileConfiguration();
            ImportErrors = new List<ImportErrorEntity>();
            SbDetail = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.AppendImportErrors(FileConfigurationAccess, ImportErrors, SbDetail));
        }

        [Test]
        public void AppendImportErrors_WhenImportErrorsIsNotNullButEmpty_ReturnsEmptyString()
        {
            // Arrange
            FileConfigurationAccess = new FileConfiguration();
            ImportErrors = new List<ImportErrorEntity>();
            SbDetail = new StringBuilder();
           
            // Act
            var result = ClientMethodHelpers.AppendImportErrors(FileConfigurationAccess, ImportErrors, SbDetail);

            // Assert
            result.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        public void AppendImportErrors_WhenImportErrorsIsNotNullAndIsQuoteEncapsulatedIsTrue_ReturnsString()
        {
            // Arrange
            CreateImportErrorsList();
            SbDetail = new StringBuilder();
            FileConfigurationAccess = new FileConfiguration
            {
                IsQuoteEncapsulated = true
            };

            SetImportErrorsResults(FileConfigurationAccess, ImportErrors);
           
            // Act
            var result = ClientMethodHelpers.AppendImportErrors(FileConfigurationAccess, ImportErrors, SbDetail);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(ReportBodyResult),
                () => SbDetail.ToString().ShouldBe(SbDetailResult.ToString()));
        }

        [Test]
        public void AppendImportErrors_WhenImportErrorsIsNotNullAndIsQuoteEncapsulatedIsFalse_ReturnsString()
        {
            // Arrange
            CreateImportErrorsList();
            SbDetail = new StringBuilder();
            FileConfigurationAccess = new FileConfiguration
            {
                IsQuoteEncapsulated = false
            };

            SetImportErrorsResults(FileConfigurationAccess, ImportErrors);
           
            // Act
            var result = ClientMethodHelpers.AppendImportErrors(FileConfigurationAccess, ImportErrors, SbDetail);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(ReportBodyResult),
                () => SbDetail.ToString().ShouldBe(SbDetailResult.ToString()));
        }

        [Test]
        public void EmailSender_WhenClientEntityIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.EmailSender(ClientEntity, EventMessage, SbDetail, SamplePubsCodes, PubCodeKey));
        }

        [Test]
        public void EmailSender_WhenEventMessageIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.EmailSender(ClientEntity, EventMessage, SbDetail, SamplePubsCodes, PubCodeKey));
        }

        [Test]
        public void EmailSender_WhenSbDetailIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved();
            SbDetail = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => ClientMethodHelpers.EmailSender(ClientEntity, EventMessage, SbDetail, SamplePubsCodes, PubCodeKey));
        }

        [Test]
        public void EmailSender_ShouldSetupMailer_SendsEmail()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved {SourceFile = new SourceFile {SourceFileID = 1}};
            SbDetail = new StringBuilder();
            ShimsForMailMessages();

            // Act, Assert
            ClientMethodHelpers.EmailSender(ClientEntity, EventMessage, SbDetail, SamplePubsCodes, PubCodeKey);
        }

        [Test]
        public void FinishingUploadToFtp_WhenClientEntityIsNull_ThrowsException()
        {
            // Arrange
            ClientEntity = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                ClientMethodHelpers.FinishingUploadToFtp(
                    ClientEntity, 
                    Consts.AutoGenSaetbMembersNtb258ENewsletterFileName, 
                    Consts.SaetbAutoGenMembersNtb258ENewsletterFilePath));
        }

        [Test]
        public void FinishingUploadToFtp_WhenClientEntityIsNotNull_ShouldUploadCsvFile()
        {
            // Arrange
            ShimForClientFtp();
            ClientEntity = new Client();
            
            // Act, Assert
            ClientMethodHelpers.FinishingUploadToFtp(
                ClientEntity, 
                Consts.AutoGenSaetbMembersNtb258ENewsletterFileName,
                Consts.SaetbAutoGenMembersNtb258ENewsletterFilePath);
        }

        private void CreateDictionaryForNewRows()
        {
            _newRows = new Dictionary<int, DataRow>
            {
                [1] = _workDataTable.Rows[0]
            };
        }

        private void CreateSampleDataTable()
        {
            _workDataTable.Columns.Add(SampleServer, typeof(string));
            _workDataTable.Columns.Add(SampleUsername, typeof(string));
            _workDataTable.Columns.Add(PubCodeKey, typeof(string));

            var dataRow = _workDataTable.NewRow();
            dataRow[PubCodeKey] = PubCodeKey;
            dataRow[SampleServer] = PubCodeKey;
            dataRow[SampleUsername] = PubCodeKey;
            _workDataTable.Rows.Add(dataRow);
        }

        private void CreateDataTableResult(int position)
        {
            _resultRows = new Dictionary<int, DataRow>();

            var newRow = _workDataTable.NewRow();
            newRow.ItemArray = _originalRows.ItemArray;
            newRow[PubCodeKey] = PubCodeUpperKey;
            _resultRows.Add(position, newRow);
        }
    }
}
