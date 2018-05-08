using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ADMS.ClientMethods;
using ADMS.ClientMethods.Fakes;
using ADMS.Services.Fakes;
using Core.ADMS.Events;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.ClientMethods.Common;
using ClientEntity = KMPlatform.Entity.Client;
using ImportVesselObject = FrameworkUAD.Object.ImportVessel;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SaetbTest : Fakes
    {
        public const int PubCodesLength = 9;
        private const string ImsProgram01 = "IMS_Program01";
        private const string ImsProgram02 = "IMS_Program02";
        private const string ImsProgram04 = "IMS_Program04";
        private const string ImsProgram06 = "IMS_Program06";
        private const string ImsProgram07 = "IMS_Program07";
        private const string ImsProgram08 = "IMS_Program08";
        private const string ImsProgram09 = "IMS_Program09";
        private const string ImsProgram10 = "IMS_Program10";
        private const string ImsProgram11 = "IMS_Program11";

        private const string Tbmin = "TBMIN";
        private const string Tbdin = "TBDIN";
        private const string Tbmcin = "TBMCIN";
        private const string Tbmdin = "TBMDIN";
        private const string Tbecin = "TBECIN";
        private const string Tbpin = "TBPIN";
        private const string Tbpmin = "TBPMIN";
        private const string Tbain = "TBAIN";
        private const string Tbtmin = "TBTMIN";

        private const string Demo31 = "DEMO31";
        private const string Demo32 = "DEMO32";
        private const string Demo33 = "DEMO33";
        private const string Demo34 = "DEMO34";
        private const string Demo35 = "DEMO35";
        private const string Demo36 = "DEMO36";

        private IDictionary<int, DataRow> _newRows;
        private DataTable _workTable;
        private int _batchProcessing;
        private int _workProcessed;
        private string _demo33;
        private string _demo34;
        private string _demo35;
        private string _demo36;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void ProcessImportVesselRows_WhenEventMessageIsNull_ThrowsException()
        {
            // Arrange
            EventMessage = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.ProcessImportVesselRows(EventMessage, FileConfigurationAccess, null, string.Empty);
            });
        }

        [Test]
        public void ProcessImportVesselRows_WhenFileConfigurationAccessIsNull_ThrowsException()
        {
            // Arrange
            EventMessage = new FileMoved();
            FileConfigurationAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.ProcessImportVesselRows(EventMessage, FileConfigurationAccess, ImportErrors, PubCodeKey);
            });
        }

        [Test]
        public void ProcessImportVesselRows_WhenFileRowsIsZero_ReturnsNullDataTable()
        {
            // Arrange
            ShimForFileRows(0);
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();

            // Act
            var result = Saetb.ProcessImportVesselRows(EventMessage, FileConfigurationAccess, ImportErrors, PubCodeKey);

            // Asssert
            result.ShouldBeNull();
        }

        [Test]
        public void ProcessImportVesselRows_ShouldProcessRows_ReturnsDataTable()
        {
            // Arrange
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();

            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            // Act
            var result = Saetb.ProcessImportVesselRows(EventMessage, FileConfigurationAccess, ImportErrors, PubCodeKey);

            // Assert
            result.ShouldBeOfType<DataTable>();
            result.Rows.Count.ShouldBe(DtAccess.Rows.Count);
        }

        [Test]
        public void CreateCsvFileForNtb258_WhenDtAccessIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.CreateCsvFileForNtb258(DtAccess);
            });
        }

        [Test]
        public void CreateCsvFileForNtb258_WhenDtAccessHasRows_ShouldCreateCsvFile()
        {
            // Arrange
            CreateDataTableAccessForImsProgram();
            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            ShimForConsoleMessage();

            // Act, Assert
            Saetb.CreateCsvFileForNtb258(DtAccess);
        }

        [Test]
        public void AddAllPubCodes_WhenDataRowIsNull_ThrowsException()
        {
            // Arrange
            DataRow dataRow = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                SAETB.AddAllPubCodes(dataRow, PubCodes);
            });
        }

        [Test]
        public void AddAllPubCodes_WhenPubCodesIsNull_ThrowsException()
        {
            // Arrange
            CreateDataTableAccessForImsProgram();
            PubCodes = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                SAETB.AddAllPubCodes(DtAccess.Rows[0], PubCodes);
            });
        }

        [Test]
        public void AddAllPubCodes_WhenPubCodesIsNotNull_ShouldFillAllPubCodes()
        {
            // Arrange
            CreateDataTableAccessForImsProgram();
            PubCodes = new List<string>();

            // Act
            SAETB.AddAllPubCodes(DtAccess.Rows[0], PubCodes);

            // Assert
            PubCodes.ShouldSatisfyAllConditions
            (
                () => PubCodes.Count.ShouldBe(PubCodesLength),
                () => PubCodes.ShouldContain(Tbmin),
                () => PubCodes.ShouldContain(Tbdin),
                () => PubCodes.ShouldContain(Tbmcin),
                () => PubCodes.ShouldContain(Tbmdin),
                () => PubCodes.ShouldContain(Tbecin),
                () => PubCodes.ShouldContain(Tbpin),
                () => PubCodes.ShouldContain(Tbpmin),
                () => PubCodes.ShouldContain(Tbain),
                () => PubCodes.ShouldContain(Tbtmin)
            );
        }

        [Test]
        public void SaetbMembersNtb258ENewsletter_WhenDataAccessIsNotNull_ShouldUploadFile()
        {
            // Arrange
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();
            ClientEntity = new ClientEntity();

            CreateDataTableAccessForImsProgram();

            ShimForCreateCsvFile();
            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimForDataTableAcceptChanges();
            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            // Act, Assert
            Saetb.SAETB_Members_NTB258_ENewsletter(
                ClientEntity,
                ClientSpecialFile,
                ClientCustomProcedure,
                EventMessage);
        }

        [Test]
        public void SaetbMembersNtb258ENewsletter_ShouldThrowsException_CreateLogError()
        {
            // Arrange
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();
            ClientEntity = new ClientEntity();

            CreateDataTableAccessForImsProgram();

            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimForDataTableAcceptChanges();
            ShimForFileRows(3);
            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (_, workTable, fileName, deleteExists) => throw new InvalidOperationException();

            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            // Act, Assert
            Saetb.SAETB_Members_NTB258_ENewsletter(
                ClientEntity,
                ClientSpecialFile,
                ClientCustomProcedure,
                EventMessage);
        }

        [Test]
        public void CreateCsvForKnws0100_WhenDtAccessIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.CreateCsvForKnws0100(DtAccess);
            });
        }

        [Test]
        public void CreateCsvForKnws0100_WhenDtAccessHasRows_ShouldCreateCsvFile()
        {
            // Arrange
            CreateDataTableAccessForDemo();
            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            ShimForConsoleMessage();

            // Act, Assert
            Saetb.CreateCsvForKnws0100(DtAccess);
        }

        [Test]
        public void AddProcessedRows_WhenDataTableAccessIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.AddProcessedRows(DtAccess, _workTable, _newRows, _workProcessed, _batchProcessing);
            });
        }

        [Test]
        public void AddProcessedRows_WhenWorkTableIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = new DataTable();
            _workTable = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.AddProcessedRows(DtAccess, _workTable, _newRows, _workProcessed, _batchProcessing);
            });
        }

        [Test]
        public void AddProcessedRows_WhenNewRowsIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = new DataTable();
            _workTable = new DataTable();
            _newRows = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                Saetb.AddProcessedRows(DtAccess, _workTable, _newRows, _workProcessed, _batchProcessing);
            });
        }

        [Test]
        public void AddProcessedRows_WhenNoEmailKeyIsYesCode_ReturnsInteger()
        {
            // Arrange
            CreateDataTableAccessForDemo();
            CreateDictionaryForNewRows();
            CreateDictionaryForWorkTable();
            _workProcessed = 0;
            _batchProcessing = 1;

            // Act
            var result = Saetb.AddProcessedRows(DtAccess, _workTable, _newRows, _workProcessed, _batchProcessing);

            // Assert
            result.ShouldBe(_batchProcessing);
        }

        [Test]
        public void AddProcessedRows_WhenNoEmailKeyIsNoCode_ReturnsInteger()
        {
            // Arrange
            CreateDataTableAccessForDemo();
            CreateDictionaryForNewRows();
            CreateDictionaryForWorkTable();

            _workProcessed = 0;
            _batchProcessing = 1;
            DtAccess.Rows[0][NoEmailKey] = NoCode;

            // Act
            var result = Saetb.AddProcessedRows(DtAccess, _workTable, _newRows, _workProcessed, _batchProcessing);

            // Assert
            result.ShouldBe(_batchProcessing);
        }

        [Test]
        public void SetConditionsForNoEmail_WhenDataTableAccessIsNull_ThrowsException()
        {
            // Arrange
            DtAccess.Columns.Add(ImsProgram01, typeof(string));
            var dataRow = DtAccess.NewRow();
            dataRow = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                SAETB.SetConditionsForNoEmail(dataRow, ref _demo33, out _demo34, ref _demo35, out _demo36);
            });
        }

        [Test]
        public void SetConditionsForNoEmail_WhenNoTelemktgAndNoAdvEmailKeyAreNull_Demo33AndDemo35ShouldBeTrue()
        {
            // Arrange
            CreateDataTableAccessForDemo();
            var dataRow = DtAccess.Rows[0];
            dataRow[NoAdvEmailKey] = string.Empty;
            dataRow[NoTelemktg] = string.Empty;

            // Act
            SAETB.SetConditionsForNoEmail(dataRow, ref _demo33, out _demo34, ref _demo35, out _demo36);

            // Assert
            dataRow.ShouldSatisfyAllConditions(
                () => _demo33.ShouldBe(bool.TrueString),
                () => _demo34.ShouldBe(bool.TrueString),
                () => _demo35.ShouldBe(bool.TrueString),
                () => _demo36.ShouldBe(bool.TrueString));
        }

        [Test]
        public void SetConditionsForNoEmail_WhenNoTelemktgAndNoAdvEmailKeyAreNoCode_Demo33AndDemo35ShouldBeTrue()
        {
            // Arrange
            CreateDataTableAccessForDemo();
            var dataRow = DtAccess.Rows[0];
            dataRow[NoAdvEmailKey] = NoCode;
            dataRow[NoTelemktg] = NoCode;

            // Act
            SAETB.SetConditionsForNoEmail(dataRow, ref _demo33, out _demo34, ref _demo35, out _demo36);

            // Assert
            dataRow.ShouldSatisfyAllConditions(
                () => _demo33.ShouldBe(bool.TrueString),
                () => _demo34.ShouldBe(bool.TrueString),
                () => _demo35.ShouldBe(bool.TrueString),
                () => _demo36.ShouldBe(bool.TrueString));
        }

        [Test]
        public void SetConditionsForNoEmail_WhenNoTelemktgAndNoAdvEmailKeyAreYesCode_Demo33AndDemo35ShouldBeFalse()
        {
            // Arrange
            CreateDataTableAccessForDemo();
            var dataRow = DtAccess.Rows[0];

            // Act
            SAETB.SetConditionsForNoEmail(dataRow, ref _demo33, out _demo34, ref _demo35, out _demo36);

            // Assert
            dataRow.ShouldSatisfyAllConditions(
                () => _demo33.ShouldBe(bool.FalseString),
                () => _demo34.ShouldBe(bool.TrueString),
                () => _demo35.ShouldBe(bool.FalseString),
                () => _demo36.ShouldBe(bool.TrueString));
        }

        [Test]
        public void SAETB_KNWS0100_WhenFileRowsThrowsException_CreateLogError()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();
            CreateDataTableAccessForDemo();

            ShimForClientFtp();
            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            // Act, Arrange
            Saetb.SAETB_KNWS0100(ClientEntity, ClientSpecialFile, ClientCustomProcedure, EventMessage);
        }

        [Test]
        public void SAETB_KNWS0100_WhenClientEntityIsNotNull_ShouldUploadFile()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved { SourceFile = new SourceFile { SourceFileID = 1 } };
            FileConfigurationAccess = new FileConfiguration();
            CreateDataTableAccessForDemo();
            CreateImportErrorsList();
            SetEmailHeaders(DtAccess);

            ShimForFileRows(3);
            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimsForMailMessages();

            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            // Act, Arrange
            Saetb.SAETB_KNWS0100(ClientEntity, ClientSpecialFile, ClientCustomProcedure, EventMessage);
        }

        [Test]
        public void GetPubCodeList_ShoudCreateListWithAllPubCodes_ReturnsStringList()
        {
            // Arrange
            var pubCodes = CreatePubCodeList();

            // Act
            var result = SAETB.GetPubCodeList();

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(pubCodes.Count),
                () => result.ShouldContain(SnwaeCode),
                () => result.ShouldContain(SnwauCode),
                () => result.ShouldContain(SnwohCode),
                () => result.ShouldContain(SnwtnaeCode),
                () => result.ShouldContain(SnwtnallCode),
                () => result.ShouldContain(SnwtnauCode),
                () => result.ShouldContain(SnwtnelCode),
                () => result.ShouldContain(SnwthdCode),
                () => result.ShouldContain(SnwtnofCode),
                () => result.ShouldContain(SaeroCode),
                () => result.ShouldContain(SautoCode),
                () => result.ShouldContain(SdigautoCode),
                () => result.ShouldContain(SofhCode),
                () => result.ShouldContain(SsaeupDomCode));
        }

        [Test]
        public void GetTransId_WhenDataRowIsNull_ThrowsException()
        {
            // Arrange, Act
            var pubCode = SampleUsername;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => SAETB.GetTransId(null, pubCode));
        }

        [Test]
        public void GetTransId_WhenPubCodeIsIncorrect_ReturnTransIdEquals10()
        {
            // Arrange
            CreateDataTableAccessForMysae(SnwaeCode, NoCode);

            // Act
            var result = SAETB.GetTransId(DtAccess.Rows[0], SampleUsername);

            // Assert
            result.ShouldBe(InitialTransId);
        }

        [Test]
        [TestCase("SNWAE")]
        [TestCase("SNWAU")]
        [TestCase("SNWOH")]
        [TestCase("SNWTNAE")]
        [TestCase("SNWTNALL")]
        [TestCase("SNWTNAU")]
        [TestCase("SNWTNEL")]
        [TestCase("SNWTNHD")]
        [TestCase("SNWTNOF")]
        public void GetTransId_WhenPubCodeIsMemberOfPubCodeList_ReturnCorrectTransId(string pubCode)
        {
            // Arrange
            CreateDataTableAccessForMysae(pubCode, NoCode);
            var resultValue = DtAccess.Rows[0][pubCode].ToString().Equals(NoCode, StringComparison.CurrentCultureIgnoreCase)
                ? DefaultTransId
                : InitialTransId;

            // Act
            var result = SAETB.GetTransId(DtAccess.Rows[0], pubCode);

            // Assert
            result.ShouldBe(resultValue);
        }

        [Test]
        [TestCase("SAERO")]
        [TestCase("SAUTO")]
        [TestCase("SDIGAUTO")]
        [TestCase("SOFH")]
        [TestCase("SSAEUP-DOM")]
        public void GetTransId_WhenPubCodeIsComplexAndMemberOfPubCodeList_ReturnCorrectTransId(string pubCode)
        {
            // Arrange
            var pubCodeName = $"MAG_{pubCode}".Replace('-', '_');

            CreateDataTableAccessForMysae(pubCodeName, NoCode);

            var resultValue = DtAccess.Rows[0][pubCodeName].ToString().Equals(NoCode, StringComparison.CurrentCultureIgnoreCase)
                ? DefaultTransId
                : InitialTransId;

            // Act
            var result = SAETB.GetTransId(DtAccess.Rows[0], pubCode);

            // Assert
            result.ShouldBe(resultValue);
        }

        [Test]
        public void SetSecondaryMysaeFields_WhenDataRowIsNull_ThrowsException()
        {
            // Arrange
            DtAccess.Columns.Add(ImsProgram01, typeof(string));
            var dataRow = DtAccess.NewRow();
            dataRow = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => SAETB.SetSecondaryMysaeFields(dataRow, _demo33, out _demo34, out _demo35));
        }

        [Test]
        public void SetSecondaryMysaeFields_WhenNoTelemktgIsEmpty_ReturnsTrue()
        {
            // Arrange
            CreateDataTableAccesForMysae(YesCode, string.Empty, YesCode, YesCode);

            // Act
            var result = SAETB.SetSecondaryMysaeFields(DtAccess.Rows[0], _demo33, out _demo34, out _demo35);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(bool.TrueString),
                () => _demo34.ShouldBe(bool.FalseString),
                () => _demo35.ShouldBe(bool.FalseString));
        }

        [Test]
        public void SetSecondaryMysaeFields_WhenNoTelemktgIsNoCode_ReturnsTrue()
        {
            // Arrange
            CreateDataTableAccesForMysae(YesCode, NoCode, YesCode, YesCode);

            // Act
            var result = SAETB.SetSecondaryMysaeFields(DtAccess.Rows[0], _demo33, out _demo34, out _demo35);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(bool.TrueString),
                () => _demo34.ShouldBe(bool.FalseString),
                () => _demo35.ShouldBe(bool.FalseString));
        }

        [Test]
        public void SetSecondaryMysaeFields_WhenNoTelemktgIsYesCode_ReturnsFalse()
        {
            // Arrange
            CreateDataTableAccesForMysae(YesCode, YesCode, YesCode, YesCode);

            // Act
            var result = SAETB.SetSecondaryMysaeFields(DtAccess.Rows[0], _demo33, out _demo34, out _demo35);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(bool.FalseString),
                () => _demo34.ShouldBe(bool.FalseString),
                () => _demo35.ShouldBe(bool.FalseString));
        }

        [Test]
        public void SetSecondaryMysaeFields_WhenNoTelemktgIsYesCodeAndNorowemlAndNoroweml3AreNoCode_ReturnsFalse()
        {
            // Arrange
            CreateDataTableAccesForMysae(YesCode, YesCode, NoCode, NoCode);

            // Act
            var result = SAETB.SetSecondaryMysaeFields(DtAccess.Rows[0], _demo33, out _demo34, out _demo35);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(bool.FalseString),
                () => _demo34.ShouldBe(bool.TrueString),
                () => _demo35.ShouldBe(bool.TrueString));
        }

        [Test]
        public void SetSecondaryMysaeFields_WhenNoTelemktgIsYesCodeAndNorowemlAndNoroweml3AreInvalid_ReturnsFalse()
        {
            // Arrange
            CreateDataTableAccesForMysae(YesCode, YesCode, SampleUsername, SampleUsername);

            // Act
            var result = SAETB.SetSecondaryMysaeFields(DtAccess.Rows[0], _demo33, out _demo34, out _demo35);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(bool.FalseString),
                () => _demo34.ShouldBe(bool.FalseString),
                () => _demo35.ShouldBe(bool.FalseString));
        }

        [Test]
        public void AddProcessedRowsForMysae_WhenPubCodeAppendListIsNull_ThrowsException()
        {
            // Arrange
            List<string> pubCodeAppendList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Saetb.AddProcessedRowsForMysae(pubCodeAppendList, DtAccess, _workTable, _newRows, 0));
        }

        [Test]
        public void AddProcessedRowsForMysae_WhenDataTableMysaeIsNull_ThrowsException()
        {
            // Arrange
            var pubCodeAppendList = new List<string> {PubCodeKey};
            DtAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Saetb.AddProcessedRowsForMysae(pubCodeAppendList, DtAccess, _workTable, _newRows, 0));
        }
        
        [Test]
        public void AddProcessedRowsForMysae_WhenWorkTableMysaeIsNull_ThrowsException()
        {
            // Arrange
            var pubCodeAppendList = new List<string> {PubCodeKey};
            CreateDataTableAccesForMysae(NoCode, YesCode, YesCode, YesCode);
            _workTable = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Saetb.AddProcessedRowsForMysae(pubCodeAppendList, DtAccess, _workTable, _newRows, 0));
        }

        [Test]
        public void AddProcessedRowsForMysae_WhenNewRowsIsNull_ThrowsException()
        {
            // Arrange
            var pubCodeAppendList = new List<string> {PubCodeKey};
            CreateDataTableAccesForMysae(NoCode, YesCode, YesCode, YesCode);
            CreateDictionaryForWorkTable();
            _newRows = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Saetb.AddProcessedRowsForMysae(pubCodeAppendList, DtAccess, _workTable, _newRows, 0));
        }

        [Test]
        [TestCase("Y")]
        [TestCase("N")]
        public void AddProcessedRowsForMysae_WhenNoEmailKeyIsCorrect_ReturnsInteger(string code)
        {
            // Arrange
            var pubCodeAppendList = new List<string> {PubCodeKey};
            CreateDataTableAccesForMysae(code, YesCode, YesCode, YesCode);
            CreateDictionaryForNewRows();
            CreateDictionaryForWorkTable();

            // Act, Assert
            Saetb.AddProcessedRowsForMysae(pubCodeAppendList, DtAccess, _workTable, _newRows, 0);
        }

        [Test]
        public void CreateCsvForMysae_WhenPubCodeAppendListIsNull_ThrowsException()
        {
            // Arrange
            List<string> pubCodeAppendList = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Saetb.CreateCsvForMysae(pubCodeAppendList, DtAccess));
        }

        [Test]
        public void CreateCsvForMysae_WhenDtAccessIsNull_ThrowsException()
        {
            // Arrange
            var pubCodeAppendList = new List<string> {PubCodeKey};
            DtAccess = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => Saetb.CreateCsvForMysae(pubCodeAppendList, DtAccess));
        }

        [Test]
        public void CreateCsvForMysae_WhenDtAccessHasRows_ShouldCreateCsvFile()
        {
            // Arrange
            var pubCodeList = new List<string> {PubCodeKey};

            CreateDataTableAccessForDemo();
            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            ShimForConsoleMessage();

            // Act
            var result = Saetb.CreateCsvForMysae(pubCodeList, DtAccess);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<DataTable>(),
                () => result.Rows.Count.ShouldBe(1),
                () => result.Rows[0].ItemArray.Length.ShouldBe(DtAccess.Rows[0].ItemArray.Length));
        }

        [Test]
        public void SAETB_MYSAE_WhenFileRowsThrowsException_CreateLogError()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved();
            FileConfigurationAccess = new FileConfiguration();
            CreateDataTableAccessForDemo();

            ShimForClientFtp();
            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };

            // Act, Arrange
            Saetb.SAETB_MYSAE(ClientEntity, ClientSpecialFile, ClientCustomProcedure, EventMessage);
        }

        [Test]
        public void SAETB_MYSAE_WhenClientEntityIsNotNull_ShouldUploadFile()
        {
            // Arrange
            ClientEntity = new ClientEntity();
            EventMessage = new FileMoved { SourceFile = new SourceFile { SourceFileID = 1 } };
            FileConfigurationAccess = new FileConfiguration();
            CreateDataTableAccessForDemo();
            CreateImportErrorsList();
            SetEmailHeaders(DtAccess);

            ShimForFileRows(3);
            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            ShimForConsoleMessage();
            ShimForClientFtp();
            ShimsForMailMessages();

            ShimSAETB.GetPubCodeList = () => new List<string> {PubCodeKey};

            ShimImportVessel.AllInstances.GetImportVesselFileInfoInt32Int32FileConfiguration =
                (_, fileInfo, startRow, takeRowCount, fileConfig) => new ImportVesselObject
                {
                    TotalRowCount = 1,
                    DataOriginal = DtAccess
                };

            // Act, Arrange
            Saetb.SAETB_MYSAE(ClientEntity, ClientSpecialFile, ClientCustomProcedure, EventMessage);
        }

        private void CreateDictionaryForNewRows()
        {
            _newRows = new Dictionary<int, DataRow>
            {
                [1] = DtAccess.Rows[0]
            };
        }

        private void CreateDictionaryForWorkTable()
        {
            _workTable = DtAccess;

            _workTable.Columns.Add(Demo31, typeof(string));
            _workTable.Columns.Add(Demo32, typeof(string));
            _workTable.Columns.Add(Demo33, typeof(string));
            _workTable.Columns.Add(Demo34, typeof(string));
            _workTable.Columns.Add(Demo35, typeof(string));
            _workTable.Columns.Add(Demo36, typeof(string));

            var dataRow = DtAccess.NewRow();
            dataRow[Demo31] = string.Empty;
            dataRow[Demo32] = string.Empty;
            dataRow[Demo33] = string.Empty;
            dataRow[Demo34] = string.Empty;
            dataRow[Demo35] = string.Empty;
            dataRow[Demo36] = string.Empty;
            _workTable.Rows.Add(dataRow);
        }

        private void CreateDataTableAccessForImsProgram()
        {
            DtAccess.Columns.Add(ImsProgram01, typeof(string));
            DtAccess.Columns.Add(ImsProgram02, typeof(string));
            DtAccess.Columns.Add(ImsProgram04, typeof(string));
            DtAccess.Columns.Add(ImsProgram06, typeof(string));
            DtAccess.Columns.Add(ImsProgram07, typeof(string));
            DtAccess.Columns.Add(ImsProgram08, typeof(string));
            DtAccess.Columns.Add(ImsProgram09, typeof(string));
            DtAccess.Columns.Add(ImsProgram10, typeof(string));
            DtAccess.Columns.Add(ImsProgram11, typeof(string));

            var dataRow = DtAccess.NewRow();
            dataRow[ImsProgram01] = YesCode;
            dataRow[ImsProgram02] = YesCode;
            dataRow[ImsProgram04] = YesCode;
            dataRow[ImsProgram06] = YesCode;
            dataRow[ImsProgram07] = YesCode;
            dataRow[ImsProgram08] = YesCode;
            dataRow[ImsProgram09] = YesCode;
            dataRow[ImsProgram10] = YesCode;
            dataRow[ImsProgram11] = YesCode;
            DtAccess.Rows.Add(dataRow);
        }

        private void CreateDataTableAccessForMysae(string columnName, string columnValue)
        {
            if (columnName.Equals(SnwtnelCode))
            {
                DtAccess.Columns.Add(SnwauCode, typeof(string));
                DtAccess.Columns.Add(SnwtnelCode, typeof(string));

                var dataRow = DtAccess.NewRow();
                dataRow[SnwauCode] = NoCode;
                dataRow[SnwtnelCode] = NoCode;
                DtAccess.Rows.Add(dataRow);
            }
            else
            {
                DtAccess.Columns.Add(columnName, typeof(string));

                var dataRow = DtAccess.NewRow();
                dataRow[columnName] = columnValue;
                DtAccess.Rows.Add(dataRow);
            }
        }

        private void CreateDataTableAccesForMysae(
            string noEmailKeyValue, 
            string noTelemktgValue, 
            string norowemlValue, 
            string noroweml3Value)
        {
            DtAccess.Columns.Add(NoEmailKey, typeof(string));
            DtAccess.Columns.Add(NoTelemktg, typeof(string));
            DtAccess.Columns.Add(Noroweml, typeof(string));
            DtAccess.Columns.Add(Noroweml3, typeof(string));
            DtAccess.Columns.Add(PubCodeUpperKey, typeof(string));
            DtAccess.Columns.Add(TransactionIdKey, typeof(string));

            var dataRow = DtAccess.NewRow();
            dataRow[NoEmailKey] = noEmailKeyValue;
            dataRow[NoTelemktg] = noTelemktgValue;
            dataRow[Noroweml] = norowemlValue;
            dataRow[Noroweml3] = noroweml3Value;
            dataRow[PubCodeUpperKey] = PubCodeKey;
            dataRow[TransactionIdKey] = 1;
            DtAccess.Rows.Add(dataRow);
        }

        private static IList<string> CreatePubCodeList()
        {
            return new List<string>
            {
                SnwaeCode,
                SnwauCode,
                SnwohCode,
                SnwtnaeCode,
                SnwtnallCode,
                SnwtnauCode,
                SnwtnelCode,
                SnwthdCode,
                SnwtnofCode,
                SaeroCode,
                SautoCode,
                SdigautoCode,
                SofhCode,
                SsaeupDomCode
            };
        }
    }
}
