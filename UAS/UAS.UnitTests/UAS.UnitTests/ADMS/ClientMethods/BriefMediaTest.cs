using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using ADMS.ClientMethods;
using ADMS.Services.Fakes;
using Core.ADMS.Events;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KM.Common.Import;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.ClientMethods.Common;
using UAS.UnitTests.Helpers;
using ClientEntity = KMPlatform.Entity.Client;
using ImportVesselObject = FrameworkUAD.Object.ImportVessel;
using UadObjects = FrameworkUAD.Object;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BriefMediaTest : Fakes
    {
        private const string DummyColumnName1 = "DummyColumnName1";
        private const int FileWorkerCount1 = 1;
        private const int FileWorkerCount250001 = 250001;
        private bool _createCsvFromDataTableInvoked = false;
        private bool _relationalInsertAccessInvoked = false;
        private bool _relationalUpdateUserInvoked = false;
        private bool _relationalUpdateTaxBehaviourInvoked = false;
        private bool _relationalUpdatePageBehaviourInvoked = false;
        private bool _relationalUpdateSearchBehaviourInvoked = false;
        private bool _relationalUpdateTopicCodeInvoked = false;

        private DataTable DtAccess = new DataTable();
        private IDisposable _shimContext;

        private void SetupGetFiles()
        {
            ShimDirectoryInfo.AllInstances.GetFiles = info =>
            {
                var fileInfos = new[]
                {
                    CreateFileInfo("Access.csv"),
                    CreateFileInfo("PageBehavior.csv"),
                    CreateFileInfo("SearchBehavior.csv"),
                    CreateFileInfo("TaxonomyBehavior.csv"),
                    CreateFileInfo("TopicCode.csv"),
                    CreateFileInfo("Users.csv")
                };
                return fileInfos;
            };
        }

        public void SetUpFakes(int fileworkerRowCount)
        {
            _shimContext = ShimsContext.Create();

            ShimClientMethods.AllInstances.BriefMedia_CreateTempCMSTablesClient =
                (methods, client) => true;

            ShimDirectoryInfo.AllInstances.ExistsGet = _ => true;

            SetupGetFiles();

            ShimFileWorker.AllInstances.GetRowCountFileInfo = (worker, info) => fileworkerRowCount;

            ShimImportVessel.AllInstances
                .GetImportVesselFileInfoInt32Int32FileConfiguration =
                (vessel, info, arg3, arg4, arg5) =>
                {
                    var importVessel = new UadObjects.ImportVessel
                    {
                        TotalRowCount = 5000
                    };
                    return importVessel;
                };

            ShimInsertUpdateMethods();

            ShimClientMethods.AllInstances.BriefMedia_Relational_CleanUpDataClient = (methods, client) => true;

            ShimClientMethods.AllInstances.BriefMedia_Relational_Get_CountClient = (methods, client) => 10;

            ShimClientMethods.AllInstances.BriefMedia_Select_Data_PagingClientInt32Int32 =
                (methods, client, arg3, arg4) => new DataTable();

            _createCsvFromDataTableInvoked = false;
            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (functions, table, arg3, arg4) => { _createCsvFromDataTableInvoked = true; };

            ShimClientMethods.AllInstances.BriefMedia_Select_DataClient = (methods, client) =>
            {
                var dt = new DataTable();
                dt.Columns.Add(DummyColumnName1);
                var row = dt.NewRow();
                row[DummyColumnName1] = string.Empty;
                dt.Rows.Add(row);
                return dt;
            };

            ShimClientMethods.AllInstances.BriefMedia_DropTempCMSTablesClient = (methods, client) => true;

            ShimClientFTP.AllInstances.SelectClientInt32 = (ftp, i) =>
            {
                var clientFtp = new ClientFTP();
                var clients = new List<ClientFTP>() { clientFtp };
                return clients;
            };
        }

        public void ShimInsertUpdateMethods()
        {
            _relationalInsertAccessInvoked = false;
            ShimClientMethods.AllInstances.BriefMedia_Relational_Insert_AccessClientDataTable =
                (methods, client, arg3) =>
                {
                    _relationalInsertAccessInvoked = true;
                    return true;
                };

            _relationalUpdateUserInvoked = false;
            ShimClientMethods.AllInstances.BriefMedia_Relational_Update_UsersClientDataTable =
                (methods, client, arg3) =>
                {
                    _relationalUpdateUserInvoked = true;
                    return true;
                };

            _relationalUpdateTaxBehaviourInvoked = false;
            ShimClientMethods.AllInstances.BriefMedia_Relational_Update_TaxBehaviorClientDataTable =
                (methods, client, arg3) =>
                {
                    _relationalUpdateTaxBehaviourInvoked = true;
                    return true;
                };

            _relationalUpdatePageBehaviourInvoked = false;
            ShimClientMethods.AllInstances.BriefMedia_Relational_Update_PageBehaviorClientDataTable =
                (methods, client, arg3) =>
                {
                    _relationalUpdatePageBehaviourInvoked = true;
                    return true;
                };

            _relationalUpdateSearchBehaviourInvoked = false;
            ShimClientMethods.AllInstances.BriefMedia_Relational_Update_SearchBehaviorClientDataTable =
                (methods, client, arg3) =>
                {
                    _relationalUpdateSearchBehaviourInvoked = true;
                    return true;
                };

            _relationalUpdateTopicCodeInvoked = false;
            ShimClientMethods.AllInstances.BriefMedia_Relational_Update_TopicCodeClientDataTable =
                (methods, client, arg3) =>
                {
                    _relationalUpdateTopicCodeInvoked = true;
                    return true;
                };
        }

        [TearDown]
        public void DisposeShimContext()
        {
            if (_shimContext != null)
            {
                _shimContext.Dispose();
            }
        }

        [Test]
        public void BriefMediaVtbDnpPhone_AdHocDimensionGroupNull_ReturnsVoid()
        {
            // Arrange
            var testObject = new BriefMedia();

            //Act Assert
            AssertBriefGroupInitialization(
                fileMoved => testObject.VtbDnpPhone(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                "VTB");
        }

        [Test]
        public void BriefMediaVtbDnpEmail_AdHocDimensionGroupNull_ReturnsVoid()
        {
            // Arrange
            var testObject = new BriefMedia();

            //Act Assert
            AssertBriefGroupInitialization(
                fileMoved => testObject.VtbDnpEmail(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                "VTB");
        }

        [Test]
        public void BriefMediaCbDnpEmail_AdHocDimensionGroupNull_ReturnsVoid()
        {
            // Arrange
            var testObject = new BriefMedia();

            //Act Assert
            AssertBriefGroupInitialization(
                fileMoved => testObject.CbDnpEmail(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                "CB");
        }

        [Test]
        public void BriefMediaCbDnpPhone_AdHocDimensionGroupNull_ReturnsVoid()
        {
            // Arrange
            var testObject = new BriefMedia();

            //Act Assert
            AssertBriefGroupInitialization(
                fileMoved => testObject.CbDnpPhone(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                "CB");
        }

        [Test]
        public void BriefMediaCbDnpPostal_AdHocDimensionGroupNull_ReturnsVoid()
        {
            // Arrange
            var testObject = new BriefMedia();

            //Act Assert
            AssertBriefGroupInitialization(
                fileMoved => testObject.CbDnpPostal(fileMoved.Client, fileMoved.SourceFile, null, fileMoved),
                "CB");
        }
        
        private static void AssertBriefGroupInitialization(Action<FileMoved> testMethod, string PubCodeExpected)
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };
                AdHocDimensionGroupPubcodeMap mapToAssert = null;

                ShimAdHocDimensionGroupPubcodeMap.AllInstances.SaveAdHocDimensionGroupPubcodeMap =
                    (_, map) =>
                    {
                        mapToAssert = map;
                        return true;
                    };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(testMethod,
                    headers: new[] { "Customer Id" },
                    values: new[] { "Customer Id Test" },
                    targets: new List<TestAdHockMethodAssertionTarget>()
                    {
                        new TestAdHockMethodAssertionTarget()
                        {
                            DimensionValue = string.Empty,
                            OperatorExpected = "equal",
                            MatchValue = "Customer Id Test",
                            UpdateUADExpected = false
                        }
                    },
                    setPubCodeSpecific: true,
                    fileConfigExpected: new FileConfiguration()
                    {
                        FileExtension = ".csv",
                        FileColumnDelimiter = "comma"
                    });

                mapToAssert.ShouldNotBeNull();
                mapToAssert.Pubcode.ShouldBe(PubCodeExpected);
            }
        }

        [Test]
        public void CreateBMRelationalFiles_FileworkerRowCount1()
        {
            //Arrange:
            var briefMedia = new BriefMedia();

            SetUpFakes(FileWorkerCount1);

            //Act:
            briefMedia.CreateBMRelationalFiles(
                new Client(), 
                new SourceFile(), 
                new ClientCustomProcedure(),
                new FileMoved());

            //Assert:
            briefMedia.ShouldSatisfyAllConditions(
                () => _createCsvFromDataTableInvoked.ShouldBeTrue(),
                () => _relationalInsertAccessInvoked.ShouldBeTrue(),
                () => _relationalUpdateUserInvoked.ShouldBeTrue(),
                () => _relationalUpdateTaxBehaviourInvoked.ShouldBeTrue(),
                () => _relationalUpdatePageBehaviourInvoked.ShouldBeTrue(),
                () => _relationalUpdateSearchBehaviourInvoked.ShouldBeTrue(),
                () => _relationalUpdateTopicCodeInvoked.ShouldBeTrue()
            );
        }

        [Test]
        public void CreateBMRelationalFiles_FileworkerRowCount250001()
        {
            //Arrange:
            var briefMedia = new BriefMedia();
            SetUpFakes(FileWorkerCount250001);

            //Act:
            briefMedia.CreateBMRelationalFiles(
                new Client(), 
                new SourceFile(), 
                new ClientCustomProcedure(),
                new FileMoved());

            //Assert:
            briefMedia.ShouldSatisfyAllConditions(
                () => _createCsvFromDataTableInvoked.ShouldBeTrue(),
                () => _relationalInsertAccessInvoked.ShouldBeTrue(),
                () => _relationalUpdateUserInvoked.ShouldBeTrue(),
                () => _relationalUpdateTaxBehaviourInvoked.ShouldBeTrue(),
                () => _relationalUpdatePageBehaviourInvoked.ShouldBeTrue(),
                () => _relationalUpdateSearchBehaviourInvoked.ShouldBeTrue(),
                () => _relationalUpdateTopicCodeInvoked.ShouldBeTrue()
            );
        }
        
        [Test]
        public void CreateCsvForBmAutoGenAlgorithms_WhenDtAccessIsNull_ThrowsException()
        {
            // Arrange
            DtAccess = null;
            
            // Act, Assert
            Should.Throw<ArgumentNullException>(() => AdmsBriefMedia.CreateCsvForBmAutoGenAlgorithms(DtAccess));
        }

        [Test]
        public void BriefMedia_Algorithms_WhenFileRowsThrowsException_CreateLogError()
        {
            // Arrange
            SetupFakes();
            ClientEntity = new ClientEntity();
            FileInfo = CreateFileInfo($"{DummyName}{Consts.CsvExtension}");

            ShimForClientFtp();
            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean =
                (_, exception, client, message, removeThread, removeQue) => { };
            
            // Act, Arrange
            AdmsBriefMedia.BriefMedia_Algorithms(ClientEntity, FileInfo, ClientSpecialFile);
        }

        [Test]
        public void BriefMedia_Algorithms_WhenClientEntityIsNotNull_ShouldUploadFile()
        {
            // Arrange
            SetupFakes();
            ClientEntity = new ClientEntity();
            FileInfo = CreateFileInfo($"{DummyName}{Consts.CsvExtension}");
            CreateDataTableAccessForBmAutoGenAlgorithms();

            ShimForFileRows(DtAccess.Rows.Count);
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
            AdmsBriefMedia.BriefMedia_Algorithms(ClientEntity, FileInfo, ClientSpecialFile);
        }

        [Test]
        public void CreateCsvForBmAutoGenAlgorithms_WhenDtAccessHasRows_ShouldCreateCsvFile()
        {
            // Arrange
            SetupFakes();
            DtAccess = new DataTable();

            CreateDataTableAccessForBmAutoGenAlgorithms();
            ShimForConsoleMessage();
            ShimForDataTableAcceptChanges();
            ShimForCreateCsvFile();
            
            // Act, Assert
            AdmsBriefMedia.CreateCsvForBmAutoGenAlgorithms(DtAccess);
        }
    }
}
