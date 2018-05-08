using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using ADMS.ClientMethods;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.ADMS.ClientMethods
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WattTest
    {
        [Test]
        public void EggAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod("E", f => testObject.EggAdHocImport(f));
            }
        }

        [Test]
        public void PetAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod("T", f => testObject.PetAdHocImport(f));
            }
        }

        [Test]
        public void SicAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.SicAdHocImport(f),
                    new[] {"SICALPHA", "BE_Selected_SIC_Code"},
                    new[] {"SICALPHA_TEST", "BE_Selected_SIC_Code_TEST"},
                    "SICALPHA_TEST",
                    "BE_Selected_SIC_Code_TEST",
                    "contains");
            }
        }

        [Test]
        public void RelationalSicAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.RelationalSicAdHocImport(
                        f.Client,
                        f.SourceFile,
                        AdmsClientMethodsHelper.BuildDataTableByParams(
                            new[] { "SICALPHA", "BE_Selected_SIC_Code" },
                            new[] { "SICALPHA_TEST", "BE_Selected_SIC_Code_TEST" })
                        ),
                    new string[0],
                    new string[0],
                    "SICALPHA_TEST",
                    "BE_Selected_SIC_Code_TEST",
                    "contains");
            }
        }

        [Test]
        public void MicAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.MicAdHocImport(
                        f.Client, f.SourceFile,
                        AdmsClientMethodsHelper.BuildDataTableByParams(
                            new[] { "PubCode", "CodeSheetValue" },
                            new[] { "test", "CodeSheetValue_Test" })),
                    new string[0],
                    new string[0],
                    "CodeSheetValue_Test",
                    "test",
                    "contains");
            }
        }

        [Test]
        public void MacAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(f => testObject.MicAdHocImport(
                        f.Client, f.SourceFile,
                        AdmsClientMethodsHelper.BuildDataTableByParams(
                            new[] { "PubCode", "CodeSheetValue" },
                            new[] { "test", "CodeSheetValue_Test" })),
                    new string[0],
                    new string[0],
                    "CodeSheetValue_Test",
                    "test",
                    "contains");
            }
        }

        [Test]
        public void TopCompanyAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(fileMoved => testObject.TopCompanyAdHocImport(fileMoved),
                    new[] { "Companies in UAD", "TOP_COMPANY_Code to be applied" },
                    new[] { "test", "CodeSheetValue_Test" },
                    "test",
                    "CodeSheetValue_Test",
                    "equal");
            }
        }

        [Test]
        public void TopCompanyAdHocImportOld_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(fileMoved => testObject.TopCompanyAdHocImportOld(fileMoved),
                    new[] { "Market", "Pubcode" },
                    new[] { "test", "CodeSheetValue_Test" },
                    "test",
                    "CodeSheetValue_Test",
                    "equal");
            }
        }

        [Test]
        public void FeedAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(fileMoved => testObject.FeedAdHocImport(fileMoved),
                    new[] { "COMPANY" },
                    new[] { "test" },
                    "F",
                    "test",
                    "contains");
            }
        }

        [Test]
        public void PoultryAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(fileMoved => testObject.PoultryAdHocImport(fileMoved),
                    new[] { "COMPANY" },
                    new[] { "test" },
                    "P",
                    "test",
                    "contains");
            }
        }

        [Test]
        public void PigAdHocImport_AdHocDimensionGroupNull_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new Watt();

                // Act Assert
                AdmsClientMethodsHelper.TestAdHockMethod(fileMoved => testObject.PigAdHocImport(fileMoved),
                    new[] { "COMPANY" },
                    new[] { "test" },
                    "S",
                    "test",
                    "contains");
            }
        }

        [Test]
        public void CreateWattRelationalFiles_DirectoryNotExist_NoGetFilesCalled()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Watt();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.WATT_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.GetFiles = info =>
                {
                    getFilesCalled = true;
                    return null;
                };

                // Act
                testObject.CreateWattRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeFalse();
            }
        }

        [Test]
        public void CreateWattRelationalFiles_DirectoryExists_NoFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Watt();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.WATT_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new FileInfo[] { };
                };

                // Act
                testObject.CreateWattRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateWattRelationalFiles_DirectoryExists_PartialFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Watt();
                var getFilesCalled = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.WATT_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("KM_WATT_LOOKUP_Market_Pubcode.xlsx"),
                        new FileInfo("KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE.xlsx"),
                    };
                };

                // Act
                testObject.CreateWattRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
            }
        }

        [Test]
        public void CreateWattRelationalFiles_DirectoryExists_AllFilesFound()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var client = new KMPlatform.Entity.Client();
                var testObject = new Watt();
                var getFilesCalled = false;
                var tempTablesCleared = false;

                ShimFileFunctions.AllInstances.ExtractZipFileFileInfoString = (functions, info, arg3) => true;
                ShimClientMethods.AllInstances.WATT_CreateTempCMSTables = methods => true;

                ShimDirectoryInfo.ConstructorString = (_, path) => { };
                ShimDirectoryInfo.AllInstances.ExistsGet = info => true;
                ShimDirectoryInfo.AllInstances.GetFiles = info => {
                    getFilesCalled = true;
                    return new[]
                    {
                        new FileInfo("KM_WATT_LOOKUP_ECN_GROUPID_PUBCODE.xlsx"),
                        new FileInfo("KM_WATT_LOOKUP_MAC_MIC.xlsx"),
                        new FileInfo("KM_WATT_LOOKUP_Market_Pubcode.xlsx"),
                        new FileInfo("KM_WATT_LOOKUP_unique_SICs_for_WATT.xlsx"),
                    };
                };

                ShimFileWorker.AllInstances.GetDataFileInfoFileConfiguration = (_, __, ___) =>
                {
                    var table = new DataTable();
                    table.Columns.Add("GROUPID");
                    return table;
                };
                ShimClientMethods.AllInstances.WATT_Get_ECN_DataString = (_, __) => new DataTable();
                ShimClientMethods.AllInstances.WATT_Relational_Process_MacMicDataTable = (_, __) => true;
                ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                    (_, __, ___, ____, _____, _6, _7) => 0;

                ShimClientMethods.AllInstances.WATT_DropTempCMSTables = methods =>
                {
                    tempTablesCleared = true;
                    return true;
                };

                ShimFtpFunctions.ConstructorStringStringString = (_, __, ___, ____) => { };
                KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.UploadStringStringBoolean =(_, __, ___, ____) => true;
                ShimClientFTP.AllInstances.SelectClientInt32 = (ftp, i) => new List<ClientFTP>()
                {
                    new ClientFTP()
                };

                // Act
                testObject.CreateWattRelationalFiles(client, null, null);

                // Assert
                getFilesCalled.ShouldBeTrue();
                tempTablesCleared.ShouldBeTrue();
            }
        }
    }
}
