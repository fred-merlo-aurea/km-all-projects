using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Fakes;
using System.Windows.Fakes;
using System.Windows.Resources;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Service;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ReportLibrary.Reports.Fakes;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using static FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using ReportUtilities = ReportLibrary.Reports.ReportUtilities;

namespace UAS.UnitTests.ReportLibrary.Reports
{
    [TestFixture]
    public class ReportUtilitiesTest : Fakes
    {
        private const string ReportCodeNameUnknown = "Unknown Report Code";
        private const string ReportCodeNameCrossTab = "Cross Tab";
        private const string ReportCodeNameSingleReponse = "Single Response";
        private const string ReportCodeNameDemoXQualification = "DemoXQualification";
        private const string ReportCodeNameGeoSingleCountry = "Geo Single Country";
        private const string ReportCodeNameGeoDomesticBreakdown = "Geo Domestic BreakDown";
        private const string ReportCodeNameGeoInternationalBreakdown = "Geo International BreakDown";
        private const string ParameterProductId = "ProductID";
        private const string ParameterProductName = "ProductName";
        private const string ParameterIssueName = "IssueName";
        private const string ParameterFilters = "Filters";
        private const string ParameterAdHocFilters = "AdHocFilters";
        private const string ParameterIssueId = "IssueID";
        private const string ParameterCountryId = "CountryId";
        private const string ParameterRow = "Row";
        private const string ParameterCol = "Col";
        private const string Space = " ";
        private const string ReportyLibraryTypeNameFormatString = "ReportLibrary.Reports.{0}, ReportLibrary";
        private const int TestProductId = 42;
        private const int TestIssueId = 24;
        private const int TestCountryId = 12345;
        private const string TestProductName = "MyTestPubName";
        private const string TestIssueName = "TestIssueName";
        private const string TestAdHocFilter = "TestAdHocFilter";
        private const string TestFilters = "TestFilters";
        private const string TestRow = "MyTestRow";
        private const string TestColumn = "MyTestCol";
        private const string ExpectedRow = "MYTESTROW";
        private const string ExpectedColumn = "MYTESTCOL";
        private const string StatesShpFile = "C:\\ADMS\\Applications\\Reporting\\states.shp";
        private const string StatesDbfFile = "C:\\ADMS\\Applications\\Reporting\\states.dbf";
        private const string WorldShpFile = "C:\\ADMS\\Applications\\Reporting\\world.shp";
        private const string WorldDbfFile = "C:\\ADMS\\Applications\\Reporting\\world.dbf";
        private const string ResultDataTableNameStates = "ResultDataTableNameStates";
        private const string ResultDataTableNameWorld = "ResultDataTableNameWorld";
        private const string StatesCsvFile = "C:\\ADMS\\Applications\\Reporting\\GeoDomesticStates.csv";
        private const string WorldCsvFile = "C:\\ADMS\\Applications\\Reporting\\GeoInternationalCountries.csv";

        private Mocks _mocks;
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _mocks = new Mocks();
            _context = ShimsContext.Create();
            SetupFakes(_mocks);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetReportSource_ReportyTypeUnknown_ParametersAdded()
        {
            // Arrange
            var code = new Code { CodeName = ReportCodeNameUnknown };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                new Report(),
                new List<Country>(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(6),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        [Test]
        public void GetReportSource_ReportyTypeCrossTab_ParametersAdded()
        {
            // Arrange
            var code = new Code { CodeName = ReportCodeNameCrossTab };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                GetReport(),
                new List<Country>(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(8),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterRow].Value.ShouldBe(ExpectedRow),
                () => reportSource.Parameters[ParameterCol].Value.ShouldBe(ExpectedColumn),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        [Test]
        [TestCase(ReportCodeNameSingleReponse)]
        [TestCase(ReportCodeNameDemoXQualification)]
        public void GetReportSource_ReportyTypeSingleResponseOrDemoX_ParametersAdded(string codeName)
        {
            // Arrange
            var code = new Code { CodeName = codeName };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                GetReport(),
                new List<Country>(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(7),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterRow].Value.ShouldBe(ExpectedRow),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        [Test]
        public void GetReportSource_ReportyTypeGeoSingleCountryNoCountryFound_CountryNotAdded()
        {
            // Arrange
            var code = new Code { CodeName = ReportCodeNameGeoSingleCountry };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                GetReport(),
                new List<Country>(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(6),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        [Test]
        public void GetReportSource_ReportyTypeGeoSingleCountryFound_CountryAdded()
        {
            // Arrange
            var code = new Code { CodeName = ReportCodeNameGeoSingleCountry };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                GetReport(),
                GetCountries(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(7),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterCountryId].Value.ShouldBe(TestCountryId),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        [Test]
        [TestCase(ReportCodeNameGeoDomesticBreakdown, StatesCsvFile, StatesShpFile, StatesDbfFile, ResultDataTableNameStates)]
        [TestCase(ReportCodeNameGeoInternationalBreakdown, WorldCsvFile, WorldShpFile, WorldDbfFile, ResultDataTableNameWorld)]
        public void GetReportSource_ReportyTypeGeoBreakddownFileExists_CsvFileCreated(
            string codeName, 
            string expectedCsvFile,
            string expectedShpFile,
            string expectedDbfFile,
            string expectedTableName)
        {
            // Arrange
            var code = new Code { CodeName = codeName };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);
            var actualFileExitsList = new List<string>();
            var actualFileDeleteList = new List<string>();
            var fileStreamCreated = false;
            string actualCsvFileToCreate = null;
            string actualDataTableName = null;
            bool? actualDeleteExisting = null;

            ShimsForGeoBreakdown();
            ShimDirectory.ExistsString = directoryName => true;
            ShimFile.ExistsString = fileName => 
                {
                    actualFileExitsList.Add(fileName);
                    return true;
                };

            ShimFile.DeleteString = fileName => 
                {
                    actualFileDeleteList.Add(fileName);
                };

            ShimFileStream.ConstructorStringFileModeFileAccess = (stream, fileName, fileMode, fileAccess) =>
                {
                    fileStreamCreated = true;
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (functions, dataTable, fileName, deleteExisting) =>
                    {
                        actualCsvFileToCreate = fileName;
                        actualDataTableName = dataTable.TableName;
                        actualDeleteExisting = deleteExisting;
                    };

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                GetReport(),
                GetCountries(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => fileStreamCreated.ShouldBeFalse(),
                () => actualCsvFileToCreate.ShouldBe(expectedCsvFile),
                () => actualDeleteExisting?.ShouldBeTrue(),
                () => actualDataTableName.ShouldBe(expectedTableName),
                () => actualFileExitsList.Count.ShouldBe(2),
                () => actualFileExitsList.ShouldContain(expectedShpFile),
                () => actualFileExitsList.ShouldContain(expectedDbfFile),
                () => actualFileDeleteList.Count.ShouldBe(0),
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(6),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        [Test]
        [TestCase(ReportCodeNameGeoDomesticBreakdown, StatesCsvFile, StatesShpFile, StatesDbfFile, ResultDataTableNameStates)]
        [TestCase(ReportCodeNameGeoInternationalBreakdown, WorldCsvFile, WorldShpFile, WorldDbfFile, ResultDataTableNameWorld)]
        public void GetReportSource_ReportyTypeGeoBreakddownFilesDoNotExist_CsvFileCreated(
            string codeName, 
            string expectedCsvFile,
            string expectedShpFile,
            string expectedDbfFile,
            string expectedTableName)
        {
            // Arrange
            var code = new Code { CodeName = codeName };
            var expectedTypeName = GetExpectedReportTypeName(code.CodeName);
            var actualFileExitsList = new List<string>();
            var actualFileDeleteList = new List<string>();
            var actualFilestreamCreatedList = new List<string>();
            string actualCsvFileToCreate = null;
            string actualDataTableName = null;
            bool? actualDeleteExisting = null;

            ShimsForGeoBreakdown();
            ShimDirectory.ExistsString = directoryName => true;
            ShimFile.ExistsString = fileName => 
                {
                    actualFileExitsList.Add(fileName);
                    return false;
                };

            ShimFile.DeleteString = fileName => 
                {
                    actualFileDeleteList.Add(fileName);
                };

            ShimFileStream.ConstructorStringFileModeFileAccess = (stream, fileName, fileMode, fileAccess) =>
                {
                    actualFilestreamCreatedList.Add(fileName);
                };

            ShimFileFunctions.AllInstances.CreateCSVFromDataTableDataTableStringBoolean =
                (functions, dataTable, fileName, deleteExisting) =>
                    {
                        actualCsvFileToCreate = fileName;
                        actualDataTableName = dataTable.TableName;
                        actualDeleteExisting = deleteExisting;
                    };

            // Act
            var reportSource = ReportUtilities.GetReportSource(
                code,
                GetReport(),
                GetCountries(),
                GetReportingXml(),
                TestProductId,
                TestProductName,
                TestIssueId,
                TestIssueName);

            // Assert
            reportSource.ShouldSatisfyAllConditions(
                () => actualFilestreamCreatedList.Count.ShouldBe(2),
                () => actualFilestreamCreatedList.ShouldContain(expectedShpFile),
                () => actualFilestreamCreatedList.ShouldContain(expectedDbfFile),
                () => actualCsvFileToCreate.ShouldBe(expectedCsvFile),
                () => actualDeleteExisting?.ShouldBeTrue(),
                () => actualDataTableName.ShouldBe(expectedTableName),
                () => actualFileExitsList.Count.ShouldBe(2),
                () => actualFileExitsList.ShouldContain(expectedShpFile),
                () => actualFileExitsList.ShouldContain(expectedDbfFile),
                () => actualFileDeleteList.Count.ShouldBe(0),
                () => reportSource.ShouldNotBeNull(),
                () => reportSource.Parameters.Count.ShouldBe(6),
                () => reportSource.TypeName.ShouldBe(expectedTypeName),
                () => reportSource.Parameters[ParameterProductId].Value.ShouldBe(TestProductId),
                () => reportSource.Parameters[ParameterProductName].Value.ShouldBe(TestProductName),
                () => reportSource.Parameters[ParameterIssueName].Value.ShouldBe(TestIssueName),
                () => reportSource.Parameters[ParameterFilters].Value.ShouldBe(TestFilters),
                () => reportSource.Parameters[ParameterAdHocFilters].Value.ShouldBe(TestAdHocFilter),
                () => reportSource.Parameters[ParameterIssueId].Value.ShouldBe(TestIssueId));
        }

        private static string GetExpectedReportTypeName(string codeName)
        {
            return string.Format(ReportyLibraryTypeNameFormatString, codeName.Replace(Space, string.Empty));
        }

        private static IList<Country> GetCountries()
        {
            var countryList = new List<Country>
                              {
                                  new Country() { ShortName = string.Empty },
                                  new Country() { ShortName = TestRow, CountryID = TestCountryId }
                              };

            return countryList;
        }

        private static Report GetReport()
        {
            var report = new Report { Row = TestRow, Column = TestColumn };

            return report;
        }

        private static ReportingXML GetReportingXml()
        {
            var reportingXml = new ReportingXML { AdHocFilters = TestAdHocFilter, Filters = TestFilters };

            return reportingXml;
        }

        private static void ShimsForGeoBreakdown()
        {
            ShimReportUtilities.GetDataTableResponseStatesAndCopiesReportingXMLInt32 = (reportingXml, issuedId) =>
                {
                    var response = new Response<DataTable>();
                    response.Status = Success;
                    response.Result = new DataTable(ResultDataTableNameStates);

                    return response;
                };

            ShimReportUtilities.GetDataTableResponseGetCountriesAndCopiesReportingXMLInt32 = (reportingXml, issuedId) =>
                {
                    var response = new Response<DataTable>();
                    response.Status = Success;
                    response.Result = new DataTable(ResultDataTableNameWorld);

                    return response;
                };

            ShimApplication.GetContentStreamUri = _ => new StreamResourceInfo(Stream.Null, string.Empty);

            ShimStream.AllInstances.Close = _ => { };

            ShimStream.AllInstances.CopyToStream = (_, __) => { };
        }
    }
}
