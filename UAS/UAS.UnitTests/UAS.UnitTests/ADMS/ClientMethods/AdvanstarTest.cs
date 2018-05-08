using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.IO.Fakes;
using ADMS.ClientMethods;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using Core.ADMS.Events;
using Core.ADMS.Events.Fakes;
using Core.ADMS.Fakes;
using FrameworkUAS.Entity;
using FrameworkUAS.Entity.Fakes;

using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;
using ShimAdHocDimension = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimension;
using ShimClientFTP = FrameworkUAS.BusinessLogic.Fakes.ShimClientFTP;
using ShimSourceFileEntity = FrameworkUAS.Entity.Fakes.ShimSourceFile;
using ShimAdHocDimensionGroupEntity = FrameworkUAS.Entity.Fakes.ShimAdHocDimensionGroup;
using ShimSourceFile = FrameworkUAS.BusinessLogic.Fakes.ShimSourceFile;
using ShimAdHocDimensionGroup = FrameworkUAS.BusinessLogic.Fakes.ShimAdHocDimensionGroup;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdvanstarTest
    {
        private const string FieldPubCode = "PUBCODE";
        private const string FieldPrdce = "PRDCDE";
        private const string FieldIndyCode = "IndyCode";
        private const string FieldCatCode = "CatCode";
        private const string FieldRegCode = "RegCode";
        private const string FieldPersonId = "Person_ID";
        private const string FieldDemoValue = "DEMOVALUE";
        private const string FieldDevice = "DEVICE";
        private const string FieldDeviceValue = "Device_Value";
        private const string FieldNewTitle = "NEWTITLE";
        private const string FieldTitle = "TITLE";
        private const string FieldTitleCode = "TITLE_CODE";
        private const string FieldGroupId = "GROUPID";
        private const string File1Txt = "1.txt";

        private IDisposable _shims;

        [SetUp]
        public void Init()
        {
            _shims = ShimsContext.Create();
            ShimAdHocDimension.AllInstances.DeleteInt32 = (dimension, i) => true;
            ShimFileMoved.AllInstances.SourceFileGet = moved => new SourceFile();
            var fileInfo = CreateFileInfo(File1Txt);
            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;
            ShimSourceFileBase.AllInstances.SourceFileIDGet = file => 1;
            ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (_, __, ___) => true;

            ShimFile.ExistsString = s => true;
            ShimFile.MoveStringString = (s, s1) => {};
            ShimFile.DeleteString = s => { };
        }

        [TearDown]
        public void Cleanup()
        {
            _shims?.Dispose();
            _shims = null;
        }

        [Test]
        public void NewTitleImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Advanstar();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.NewTitleImport(f),
                    new[] { "TITLE", "NEWTITLE" },
                    new[] { "Title_TEST", "NewTitle_TEST" },
                    "NewTitle_TEST",
                    "Title_TEST",
                    "equal");
            }
        }

        [Test]
        public void TitleCodeImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Advanstar();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.TitleCodeImport(f),
                    new[] { "TITLE", "TITLE_CODE" },
                    new[] { "Title_TEST", "TITLE_CODE_test" },
                    "TITLE_CODE_test",
                    "Title_TEST",
                    "equal");
            }
        }

        [Test]
        public void AdvanstarRelationalFiles_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Advanstar();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };
                ShimClientMethods.AllInstances.Advanstar_CreateTempTables = _ => true;
                ShimClientMethods.AllInstances.Advanstar_DropTempTables = _ => true;
                ShimClientMethods.AllInstances.Advanstar_Insert_PersonIDDataTable = (_, __) => true;
                ShimClientMethods.AllInstances.Advanstar_Insert_PersonID_Final = _ => null;
                ShimClientMethods.AllInstances.Advanstar_Get_CountString = (_, __) => 1;
                ShimClientMethods.AllInstances.Advanstar_Get_ECN_CountString = (_, __) => 0;
                ShimClientMethods.AllInstances.Advanstar_Insert_RegCodeCompareDataTable = (_, __) => true;
                ShimClientMethods.AllInstances.Advanstar_Insert_RegCodeDataTable = (_, __) => true;
                ShimClientMethods.AllInstances.Advanstar_Insert_RegCode_Final = _ => null;
                ShimClientMethods.AllInstances.Advanstar_Select_Data_PagingInt32Int32 = (_, __, ___) =>
                    CreateOneRowDataTable();
                ShimClientMethods.AllInstances.Advanstar_Select_Data_PagingRegCodeInt32Int32 = (_, __, ___) =>
                    CreateOneRowDataTable();
                ShimClientMethods.AllInstances.Advanstar_Select_PRDCDES = _ => new DataTable();

                ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean = (_, __, ___, _4) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = _ => new[]
                {
                    new FileInfo("KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE.xlsx"),
                    new FileInfo("KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE.xlsx"),
                    new FileInfo("KM_ADVANSTAR_LOOKUP_NEWTITLE.xlsx"),
                    new FileInfo("KM_ADVANSTAR_LOOKUP_REGCODE.xlsx"),
                    new FileInfo("KM_ADVANSTAR_LOOKUP_TITLECDE.xlsx"),
                    new FileInfo("KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE.xlsx"),
                    new FileInfo("PERSON_ID_SOURCECODE.DBF"),
                    new FileInfo("CBI_REGCODE.DBF"),
                    new FileInfo("CBI_SOURCECODE.DBF"),
                    new FileInfo("CBI_SRCODE_PRICODE.DBF")
                };

                ShimSourceFile.AllInstances.SelectInt32StringBoolean = (_, __, ___, _4) => new SourceFile();
                ShimAdHocDimension.AllInstances.DeleteInt32 = (_, __) => true;
                ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (_, data) => true;

                ShimClientFTP.AllInstances.SelectClientInt32 = (_, __) =>
                {
                    return new List<ClientFTP>
                    {
                        new ClientFTP()
                    };
                };
                ShimFtpFunctions.ConstructorStringStringString = (_, __, ___, _4) => { };
                KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.UploadStringStringBoolean = (_, __, ___, _4) => true;

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.AdvanstarRelationalFiles(f.Client, f),
                    new[] { "TITLE", "TITLE_CODE", "IndyCode", "Person_ID", "CatCode", "RegCode", "DEMOVALUE", "DEVICE", "NEWTITLE" },
                    new[] { "Title_TEST", "TITLE_CODE_test", "test", "test", "CatCode_Test", "RegCode_Test", "DEMOVALUE_TEST", "DEVICE_TEST", "NEWTITLE_TEST" },
                    new List<TestAdHockMethodAssertionTarget>()
                    {
                        new TestAdHockMethodAssertionTarget()
                        {
                            DimensionValue = "test",
                            MatchValue = "test",
                            OperatorExpected = "equals",
                            UpdateUADExpected = true
                        },
                        new TestAdHockMethodAssertionTarget()
                        {
                            DimensionValue = "CatCode_Test",
                            MatchValue = "test",
                            OperatorExpected = "equal",
                            UpdateUADExpected = true
                        },
                        new TestAdHockMethodAssertionTarget()
                        {
                            DimensionValue = "RegCode_Test",
                            MatchValue = "test",
                            OperatorExpected = "equals",
                            UpdateUADExpected = true
                        },
                        new TestAdHockMethodAssertionTarget()
                        {
                            DimensionValue = "DEMOVALUE_TEST",
                            MatchValue = "DEVICE_TEST",
                            OperatorExpected = "equal",
                            UpdateUADExpected = true
                        },
                        new TestAdHockMethodAssertionTarget()
                        {
                            DimensionValue = "NEWTITLE_TEST",
                            MatchValue = "Title_TEST",
                            OperatorExpected = "equal",
                            UpdateUADExpected = true
                        },
                        new TestAdHockMethodAssertionTarget()
                        {
                        DimensionValue = "TITLE_CODE_test",
                        MatchValue = "Title_TEST",
                        OperatorExpected = "contains",
                        UpdateUADExpected = true
                        }
                    },
                    false);
            }
        }

        [Test]
        public void AdvanstarAdvanstarRelationalFiles_FileNotExist_MainLogicOmmited()
        {
            // Arrange
            ShimBaseDirs.getClientArchiveDir = () => "";

            ShimClientMethods.AllInstances.Advanstar_CreateTempTables = _ => true;

            var advanstarInsertPersonIdDataTableCalled = false;
            ShimClientMethods.AllInstances.Advanstar_Insert_PersonIDDataTable = (methods, table) =>
            {
                advanstarInsertPersonIdDataTableCalled = true;
                return true;
            };

            var client = new Client();

            // Act
            var adv = new Advanstar();
            adv.AdvanstarRelationalFiles(client, new FileMoved());

            // Assert
            Assert.IsFalse(advanstarInsertPersonIdDataTableCalled);
        }

        [Test]
        public void AdvanstarAdvanstarRelationalFiles_AgWorkerSelectInstance_AdvanstarPersonIDIndyCode()
        {
            // Arrange
            ShimBaseDirs.getClientArchiveDir = () => "";

            ShimClientMethods.AllInstances.Advanstar_CreateTempTables = _ => true;

            var advanstarInsertPersonIdDataTableCalled = false;
            ShimClientMethods.AllInstances.Advanstar_Insert_PersonIDDataTable = (methods, table) =>
            {
                advanstarInsertPersonIdDataTableCalled = true;
                return true;
            };

            ShimDirectoryInfo.AllInstances.ExistsGet = _ => true;
            ShimFileInfo.ConstructorString = (info, s) => { };
            ShimFileInfo.ConstructorString = (info, s) => { };

            SetupGetFiles();

            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) => new DataTable();

            ShimClientMethods.AllInstances.Advanstar_Insert_PersonID_Final = methods =>
            {
                var dt = new DataTable();
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_Insert_RegCode_Final = methods => new DataTable();

            ShimClientMethods.AllInstances.Advanstar_Get_CountString = (methods, s) => 0;

            ShimClientMethods.AllInstances.Advanstar_Get_ECN_CountString = (methods, s) => 0;

            ShimClientMethods.AllInstances.Advanstar_Select_PRDCDES = methods => new DataTable();

            ShimClientMethods.AllInstances.Advanstar_DropTempTables = methods => true;

            ShimClientFTP.AllInstances.SelectClientInt32 = (ftp, i) =>
            {
                var clientFtp = new ClientFTP();
                var clients = new List<ClientFTP> { clientFtp };
                return clients;
            };

            ShimSourceFile.AllInstances.SelectInt32StringBoolean = (file, i, arg3, arg4) =>
            {
                var sf = new SourceFile();
                return sf;
            };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
            {
                adHocDimensionGroupSaved = true;
                return true;
            };

            ShimAdHocDimension.AllInstances.DeleteInt32 = (dimension, i) => true;

            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                var r = new AdHocDimensionGroup();
                return r;
            };

            ShimFileMoved.AllInstances.SourceFileGet = moved => new SourceFile();
            ShimSourceFileBase.AllInstances.SourceFileIDGet = file => 1;

            var client = new Client();

            var adv = new Advanstar();

            // Act
            adv.AdvanstarRelationalFiles(client, new FileMoved());

            // Assert
            Assert.IsTrue(advanstarInsertPersonIdDataTableCalled);
            Assert.IsFalse(adHocDimensionGroupSaved);
        }

        [Test]
        public void AdvanstarAdvanstarRelationalFiles_CountPositive_MainLogicWorks()
        {
            // Arrange
            ShimBaseDirs.getClientArchiveDir = () => "";

            ShimClientMethods.AllInstances.Advanstar_CreateTempTables = _ => true;

            ShimDirectoryInfo.AllInstances.ExistsGet = _ => true;

            ShimFileInfo.ConstructorString = (info, s) => { };

            SetupGetFiles();

            SetupShimFileWorkerGetData();

            ShimClientMethods.AllInstances.Advanstar_Insert_PersonIDDataTable = (methods, table) => true;

            ShimClientMethods.AllInstances.Advanstar_Insert_PersonID_Final = methods =>
            {
                var dt = new DataTable();
                var col = dt.Columns.Add(FieldPubCode);
                dt.PrimaryKey = new[] { col };
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_Insert_RegCodeDataTable = (methods, table) => true;

            ShimClientMethods.AllInstances.Advanstar_Insert_RegCode_Final = methods =>
            {
                var dt = new DataTable();
                var col = dt.Columns.Add(FieldPubCode);
                dt.PrimaryKey = new[] { col };
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_Get_CountString = (methods, s) => 1;

            ShimClientMethods.AllInstances.Advanstar_Get_ECN_CountString = (methods, s) => 2;

            ShimClientMethods.AllInstances.Advanstar_Select_PRDCDES = methods =>
            {
                var dt = new DataTable();
                var col = dt.Columns.Add(FieldPrdce);
                dt.Columns.Add(FieldPubCode);
                dt.PrimaryKey = new[] { col };
                var row = dt.NewRow();
                row[FieldPrdce] = string.Empty;
                dt.Rows.Add(row);
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_DropTempTables = methods => true;

            ShimClientMethods.AllInstances.Advanstar_Select_Data_PagingInt32Int32 = (methods, i, arg3) =>
            {
                var dt = new DataTable();
                var col = dt.Columns.Add(FieldPrdce);
                dt.Columns.Add(FieldPubCode);
                dt.PrimaryKey = new[] { col };
                var row = dt.NewRow();
                row[FieldPrdce] = string.Empty;
                dt.Rows.Add(row);
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_Select_Data_PagingRegCodeInt32Int32 = (methods, i, arg3) =>
            {
                var dt = new DataTable();
                var col = dt.Columns.Add(FieldPrdce);
                dt.Columns.Add(FieldPubCode);
                dt.PrimaryKey = new[] { col };
                var row = dt.NewRow();
                row[FieldPrdce] = string.Empty;
                dt.Rows.Add(row);
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_Select_ECN_PagingInt32Int32String = (methods, i, arg3, arg4) =>
            {
                var dt = new DataTable();
                var col = dt.Columns.Add(FieldPrdce);
                dt.Columns.Add(FieldGroupId);
                dt.PrimaryKey = new[] { col };
                var row = dt.NewRow();
                row[FieldPrdce] = string.Empty;
                row[FieldGroupId] = string.Empty;
                dt.Rows.Add(row);
                return dt;
            };

            ShimClientMethods.AllInstances.Advanstar_Insert_RegCodeCompareDataTable = (methods, table) => true;

            ShimClientFTP.AllInstances.SelectClientInt32 = (ftp, i) =>
            {
                var clientFtp = new ClientFTP();
                var clients = new List<ClientFTP> { clientFtp };
                return clients;
            };

            ShimSourceFile.AllInstances.SelectInt32StringBoolean = (file, i, arg3, arg4) =>
            {
                var sf = new SourceFile();
                return sf;
            };

            ShimAdHocDimension.AllInstances.DeleteInt32 = (dimension, i) => true;

            var adHocDimensionGroupSelectQty = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                if ((adHocDimensionGroupSelectQty++ % 2) == 0)
                {
                    return null;
                }

                var r = new AdHocDimensionGroup();
                return r;
            };

            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) => true;

            ShimFileMoved.AllInstances.SourceFileGet = moved => new SourceFile();

            var fileFunctionsCreateCsvFromDataCalled = false;
            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (functions, table, arg3, arg4) => { fileFunctionsCreateCsvFromDataCalled = true; };

            ShimSourceFileBase.AllInstances.SourceFileIDGet = file => 1;

            ShimAdHocDimensionGroupEntity.AllInstances.AdHocDimensionGroupIdGet = group => 1;

            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
            {
                if (list.Any())
                {
                    adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                }
                return true;
            };

            var client = new Client();

            var adv = new Advanstar();

            // Act
            adv.AdvanstarRelationalFiles(client, new FileMoved());

            // Assert
            Assert.IsTrue(fileFunctionsCreateCsvFromDataCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }

        [Test]
        public void AdvanstarNewTitleImport_AgWorkerReturnsNullDataRowPresent_AgWorkerSavedAgDataInserted()
        {
            // Arrange
            var adHocDimensionGroupSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                if ((adHocDimensionGroupSelectCnt++ % 2) == 0)
                {
                    return null;
                }

                var r = new AdHocDimensionGroup();
                return r;
            };

            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new DataTable();
                dt.Columns.Add(FieldNewTitle);
                dt.Columns.Add(FieldTitle);
                var r = dt.NewRow();
                r[FieldNewTitle] = "NewTitle";
                dt.Rows.Add(r);
                return dt;
            };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
            {
                adHocDimensionGroupSaved = true;
                return true;
            };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            var adHocDimensionSaveBulkSqlInsertTestRegionCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
            {
                adHocDimensionSaveBulkSqlInsertCalled = true;
                if (list.Any())
                {
                    adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                }

                if (list.Any(e => e.DimensionValue == "NewTitle"))
                {
                    adHocDimensionSaveBulkSqlInsertTestRegionCalled = true;
                }
                return true;
            };

            var client = new Client();
            var adv = new Advanstar();

            var fileInfo = new FileInfo(File1Txt);
            var fileMoved = new FileMoved { Client = client, ImportFile = fileInfo };

            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;

            // Act
            adv.NewTitleImport(fileMoved);

            // Assert
            Assert.AreEqual(2, adHocDimensionGroupSelectCnt);
            Assert.IsTrue(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertTestRegionCalled);
        }

        [Test]
        public void AdvanstarNewTitleImport_AgWorkerReturnsNotNullDataRowAbsent_AgWorkerNotSavedAgDataNot()
        {
            // Arrange
            var adHocDimensionGroupSelectCnt = 0;
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Int32StringBoolean = (group, i, arg3, arg4, arg5) =>
            {
                adHocDimensionGroupSelectCnt++;

                var r = new AdHocDimensionGroup();
                return r;
            };

            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new DataTable();
                dt.Columns.Add(FieldNewTitle);
                dt.Columns.Add(FieldTitle);
                return dt;
            };

            var adHocDimensionGroupSaved = false;
            ShimAdHocDimensionGroup.AllInstances.SaveAdHocDimensionGroup = (group, ahg) =>
            {
                adHocDimensionGroupSaved = true;
                return true;
            };

            var adHocDimensionSaveBulkSqlInsertCalled = false;
            var adHocDimensionSaveBulkSqlInsertNonEmptyCalled = false;
            ShimAdHocDimension.AllInstances.SaveBulkSqlInsertListOfAdHocDimension = (dimension, list) =>
            {
                adHocDimensionSaveBulkSqlInsertCalled = true;
                if (list.Any())
                {
                    adHocDimensionSaveBulkSqlInsertNonEmptyCalled = true;
                }

                return true;
            };

            var client = new Client();
            var adv = new Advanstar();

            var fileInfo = new FileInfo(File1Txt);
            var fileMoved = new FileMoved { Client = client, ImportFile = fileInfo };

            ShimFileMoved.AllInstances.ImportFileGet = moved => fileInfo;

            // Act
            adv.NewTitleImport(fileMoved);

            // Assert
            Assert.AreEqual(1, adHocDimensionGroupSelectCnt);
            Assert.IsFalse(adHocDimensionGroupSaved);
            Assert.IsTrue(adHocDimensionSaveBulkSqlInsertCalled);
            Assert.IsFalse(adHocDimensionSaveBulkSqlInsertNonEmptyCalled);
        }        

        internal static FileInfo CreateFileInfo(string name)
        {
            var shimFileInfo = new System.IO.Fakes.ShimFileInfo();
            shimFileInfo.NameGet = () => name;

            FileInfo fileInfo = shimFileInfo;

            return fileInfo;
        }       

        [Test]
        public void BadPhoneImport_AdHocDimensionGroupPhone_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Advanstar();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.BadPhoneImport(f),
                    new[] { "Device", "Device_Value" },
                    new[] { "PHONE", "TITLE_CODE_test" },
                    "false",
                    "TITLE_CODE_test",
                    "equal");
            }
        }

        [Test]
        public void BadPhoneImport_AdHocDimensionGroupFax_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Advanstar();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.BadPhoneImport(f),
                    new[] { "Device", "Device_Value" },
                    new[] { "FAX", "TITLE_CODE_test" },
                    "false",
                    "TITLE_CODE_test",
                    "equal");
            }
        }

        [Test]
        public void BadPhoneImport_AdHocDimensionGroupMobile_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Advanstar();
                ShimFile.ExistsString = _ => true;
                ShimFile.MoveStringString = (_, __) => { };

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.BadPhoneImport(f),
                    new[] { "Device", "Device_Value" },
                    new[] { "MOBILE", "TITLE_CODE_test" },
                    "false",
                    "TITLE_CODE_test",
                    "equal");
            }
        }

        private static void SetupShimFileWorkerGetData()
        {
            ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (worker, info, arg3) =>
            {
                var dt = new DataTable();
                dt.Columns.Add(FieldPubCode);
                var col2 = dt.Columns.Add(FieldPrdce);
                dt.Columns.Add(FieldIndyCode);
                dt.Columns.Add(FieldCatCode);
                dt.Columns.Add(FieldRegCode);
                dt.Columns.Add(FieldPersonId);
                dt.Columns.Add(FieldDemoValue);
                dt.Columns.Add(FieldDevice);
                dt.Columns.Add(FieldDeviceValue);
                dt.Columns.Add(FieldNewTitle);
                dt.Columns.Add(FieldTitle);
                dt.Columns.Add(FieldTitleCode);
                dt.Columns.Add(FieldGroupId);
                dt.PrimaryKey = new[] { col2 };
                var row = dt.NewRow();
                row[FieldPubCode] = 1;
                row[FieldPrdce] = 1;
                row[FieldIndyCode] = 1;
                row[FieldCatCode] = 1;
                row[FieldRegCode] = 1;
                row[FieldPersonId] = 1;
                row[FieldDemoValue] = 1;
                row[FieldDevice] = 1;
                row[FieldDeviceValue] = 1;
                row[FieldNewTitle] = 1;
                row[FieldTitle] = 1;
                row[FieldTitleCode] = 1;
                row[FieldGroupId] = 1;
                dt.Rows.Add(row);
                return dt;
            };
        }        

        private static DataTable CreateOneRowDataTable()
        {
            var page = new DataTable();
            page.Rows.Add();
            return page;
        }

        private void SetupGetFiles()
        {
            ShimDirectoryInfo.AllInstances.GetFiles = info =>
            {
                var fileInfos = new[]
                                    {
                                            CreateFileInfo("KM_ADVANSTAR_LOOKUP_BAD_PHONE_FAX_MOBILE.xlsx"),
                                            CreateFileInfo("KM_ADVANSTAR_LOOKUP_GROUPID_PUBCODE_TYPE.xlsx"),
                                            CreateFileInfo("KM_ADVANSTAR_LOOKUP_NEWTITLE.xlsx"),
                                            CreateFileInfo("KM_ADVANSTAR_LOOKUP_TITLECDE.xlsx"),
                                            CreateFileInfo("KM_ADVANSTAR_LOOKUP_WEB_AND_WP_PUBCODES_FROM_PRDCDE.xlsx"),
                                            CreateFileInfo("PERSON_ID_SOURCECODE.DBF"), CreateFileInfo("CBI_REGCODE.DBF"),
                                            CreateFileInfo("CBI_SOURCECODE.DBF"), CreateFileInfo("CBI_SRCODE_PRICODE.DBF")
                                        };
                return fileInfos;
            };
        }
    }
}