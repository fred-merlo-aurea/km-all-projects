using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities;
using FrameworkUAD.Object;
using KMPlatform.Object;
using NUnit.Framework;
using UAS.UnitTests.UAD_WS.Service.Common;
using EntityReport = FrameworkUAD.Entity.Report;
using EntityReporting = FrameworkUAD.Object.Reporting;
using ServiceReports = UAD_WS.Service.Reports;
using ShimWorker = FrameworkUAD.BusinessLogic.Fakes.ShimReport;

namespace UAS.UnitTests.UAD_WS.Service
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReportsTest : Fakes
    {
        private const int SampleId = 100;
        private const int IssueId = 200;
        private const int AffectedCountPositive = 1;
        private const string SampleString = "text1";

        private ServiceReports _testEntity;
        private DataTable _dataTable;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            _testEntity = new ServiceReports();
            _dataTable = new DataTable();
        }

        [TearDown]
        public void TearDown()
        {
            DisposeContext();
            _dataTable.Dispose();
        }

        [Test]
        public void SelectDemoSubReport_WorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectDemoSubReportClientConnectionsInt32StringBooleanStringStringInt32
                = (a, b, c, d, e, f, g, h) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SelectDemoSubReport(Guid.Empty, new ClientConnections(), SampleId, string.Empty, false, string.Empty, string.Empty, IssueId);

            // Assert
            VerifyErrorResponse(result, null, StringFunctions.FriendlyServiceError());
        }

        [Test]
        public void SelectDemoSubReport_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectDemoSubReportClientConnectionsInt32StringBooleanStringStringInt32
                = (a, b, c, d, e, f, g, h) => _dataTable;

            // Act
            var result = _testEntity.SelectDemoSubReport(Guid.Empty, new ClientConnections(), SampleId, string.Empty, false, string.Empty, string.Empty, IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetResponses_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetResponsesClientConnectionsInt32 = (_, __, ___) => _dataTable;

            // Act
            var result = _testEntity.GetResponses(Guid.Empty, new ClientConnections(), SampleId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetProfileFields_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetProfileFieldsClientConnections = (_, __) => _dataTable;

            // Act
            var result = _testEntity.GetProfileFields(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectGeoBreakdownInternational_ByFiltersAndIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectGeoBreakdownInternationalClientConnectionsStringStringInt32 = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.SelectGeoBreakdownInternational(Guid.Empty, new ClientConnections(), string.Empty, string.Empty, IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectListReport_ByReportId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityReporting();
            ShimWorker.AllInstances.SelectListReportClientConnectionsReportingInt32StringStringBoolean = (a, b, c, d, e, f, g) => _dataTable;

            // Act
            var result = _testEntity.SelectListReport(Guid.Empty, new ClientConnections(), SampleId, string.Empty, entity, string.Empty, false);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectPar3c_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectPar3cClientConnectionsStringStringInt32 = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.SelectPar3c(Guid.Empty, new ClientConnections(), string.Empty, string.Empty, IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectSubFields_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectSubFieldsClientConnectionsStringStringStringInt32 = (a, b, c, d, e, g) => _dataTable;

            // Act
            var result = _testEntity.SelectSubFields(Guid.Empty, new ClientConnections(), string.Empty, string.Empty, string.Empty, IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetSubscriberDetails_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetSubscriberDetailsClientConnectionsStringStringInt32 = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.GetSubscriberDetails(Guid.Empty, new ClientConnections(), string.Empty, string.Empty, IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectQualificationBreakDown_ByReporting_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<QualificationBreakdownReport>();
            var entity = new EntityReporting();
            ShimWorker.AllInstances.SelectQualificationBreakDownClientConnectionsReportingStringBooleanInt32 =
                (a, b, c, d, e, f) => list;

            // Act
            var result = _testEntity.SelectQualificationBreakDown(Guid.Empty, new ClientConnections(), entity, string.Empty, false, IssueId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectSubscriberCopies_ByReporting_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<int>();
            var entity = new EntityReporting();
            ShimWorker.AllInstances.SelectSubscriberCopiesReportingClientConnections =
                (a, b, c) => list;

            // Act
            var result = _testEntity.SelectSubscriberCopies(Guid.Empty, entity, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectIssueSplitsActiveCounts_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new Counts();
            ShimWorker.AllInstances.SelectActiveIssueSplitsCountsInt32ClientConnections = (a, b, c) => entity;

            // Act
            var result = _testEntity.SelectIssueSplitsActiveCounts(Guid.Empty, SampleId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, entity);
        }

        [Test]
        public void Get_Countries_And_Copies_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetCountryAndCopiesStringInt32ClientConnections = (a, b, c, d) => _dataTable;

            // Act
            var result = _testEntity.Get_Countries_And_Copies(Guid.Empty, string.Empty, IssueId, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void Select_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityReport>();
            ShimWorker.AllInstances.SelectClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.Select(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Select_For_AddRemove_Reports_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<EntityReport>();
            ShimWorker.AllInstances.SelectForAddRemovesClientConnectionsInt32 = (a, b, c) => list;

            // Act
            var result = _testEntity.Select_For_AddRemove_Reports(Guid.Empty, new ClientConnections(), SampleId);

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Save_WithEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityReport();
            ShimWorker.AllInstances.SaveClientConnectionsReport = (a, b, c) => AffectedCountPositive;
            ShimForJsonFunction<EntityReport>();

            // Act
            var result = _testEntity.Save(Guid.Empty, new ClientConnections(), entity);

            // Assert
            VerifySuccessResponse(result, AffectedCountPositive);
        }

        [Test]
        public void SelectBPA_WithReportingEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityReporting();
            ShimWorker.AllInstances.SelectBPAClientConnectionsReportingStringBoolean = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.SelectBPA(Guid.Empty, new ClientConnections(), entity, SampleString, true);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectCategorySummary_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectCategorySummaryClientConnectionsStringStringInt32 = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.SelectCategorySummary(Guid.Empty, new ClientConnections(), SampleString, SampleString, IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectCrossTab_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectCrossTabClientConnectionsInt32StringStringBooleanStringStringInt32Boolean
                = (a, b, c, d, e, f, g, h, i, j) => _dataTable;

            // Act
            var result = _testEntity.SelectCrossTab(
                Guid.Empty,
                new ClientConnections(),
                SampleId,
                SampleString,
                SampleString,
                true,
                SampleString,
                SampleString,
                IssueId,
                true);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectSingleResponse_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectSingleResponseClientConnectionsInt32StringBooleanStringStringInt32
                = (a, b, c, d, e, f, g, h) => _dataTable;

            // Act
            var result = _testEntity.SelectSingleResponse(
                Guid.Empty,
                new ClientConnections(),
                SampleId,
                SampleString,
                true,
                SampleString,
                SampleString,
                IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectDemoXQualification_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectDemoXQualificationClientConnectionsInt32StringStringStringInt32Boolean
                = (a, b, c, d, e, f, g, h) => _dataTable;

            // Act
            var result = _testEntity.SelectDemoXQualification(
                Guid.Empty,
                new ClientConnections(),
                SampleId,
                SampleString,
                SampleString,
                SampleString,
                IssueId,
                true);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetIssueDates_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetIssueDatesClientConnectionsInt32 = (a, b, c) => _dataTable;

            // Act
            var result = _testEntity.GetIssueDates(Guid.Empty, new ClientConnections(), SampleId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectGeoBreakdown_Single_Country_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectGeoBreakdown_Single_CountryClientConnectionsStringStringInt32Int32
                = (a, b, c, d, e, f) => _dataTable;

            // Act
            var result = _testEntity.SelectGeoBreakdown_Single_Country(
                Guid.Empty,
                new ClientConnections(),
                SampleString,
                SampleString,
                IssueId,
                SampleId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectGeoBreakdown_Domestic_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectGeoBreakdown_DomesticClientConnectionsStringStringInt32Boolean
                = (a, b, c, d, e, f) => _dataTable;

            // Act
            var result = _testEntity.SelectGeoBreakdown_Domestic(
                Guid.Empty,
                new ClientConnections(),
                SampleString,
                SampleString,
                IssueId,
                true);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetCountries_ByAccessKey_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetCountriesClientConnections = (a, b) => _dataTable;

            // Act
            var result = _testEntity.GetCountries(Guid.Empty, new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectQSourceBreakdown_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectQSourceBreakdownClientConnectionsInt32BooleanStringStringInt32
                = (a, b, c, d, e, f, g) => _dataTable;

            // Act
            var result = _testEntity.SelectQSourceBreakdown(
                Guid.Empty,
                new ClientConnections(),
                SampleId,
                true,
                SampleString,
                SampleString,
                IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectSubsrc_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.SelectSubsrcClientConnectionsStringStringBooleanInt32
                = (a, b, c, d, e, f) => _dataTable;

            // Act
            var result = _testEntity.SelectSubsrc(
                Guid.Empty,
                new ClientConnections(),
                SampleString,
                SampleString,
                true,
                IssueId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectAddRemove_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = new EntityReporting();
            ShimWorker.AllInstances.SelectAddRemoveClientConnectionsReportingInt32StringBoolean
                = (a, b, c, d, e, f) => _dataTable;

            // Act
            var result = _testEntity.SelectAddRemove(
                Guid.Empty,
                new ClientConnections(),
                entity,
                IssueId,
                SampleString,
                true);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void ReqFlagSummary_ByProductId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.ReqFlagSummaryClientConnectionsInt32 = (a, b, c) => _dataTable;

            // Act
            var result = _testEntity.ReqFlagSummary(Guid.Empty, new ClientConnections(), SampleId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetFullSubscriberDetails_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetFullSubscriberDetailsClientConnectionsStringStringInt32
                = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.GetFullSubscriberDetails(
                Guid.Empty,
                new ClientConnections(),
                SampleString,
                SampleString,
                SampleId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetSubscriberPaidDetails_ByFilters_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetSubscriberPaidDetailsClientConnectionsStringString = (a, b, c, d) => _dataTable;

            // Act
            var result = _testEntity.GetSubscriberPaidDetails(Guid.Empty, new ClientConnections(), SampleString, SampleString);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void GetSubscriberResponseDetails_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetSubscriberResponseDetailsClientConnectionsStringStringInt32
                = (a, b, c, d, e) => _dataTable;

            // Act
            var result = _testEntity.GetSubscriberResponseDetails(
                Guid.Empty,
                new ClientConnections(),
                SampleString,
                SampleString,
                SampleId);

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }

        [Test]
        public void SelectSubscriberCount_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<int>();
            ShimWorker.AllInstances.SelectSubscriberCountStringStringBooleanBooleanInt32ClientConnections
                = (a, b, c, d, e, f, g) => list;

            // Act
            var result = _testEntity.SelectSubscriberCount(
                Guid.Empty,
                SampleString,
                SampleString,
                true,
                true,
                SampleId,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void SelectSubCountUAD_WithReportingEntity_ReturnsSuccessResponse()
        {
            // Arrange
            var list = new List<int>();
            ShimWorker.AllInstances.SelectSubCountUADReportingClientConnections = (a, b, c) => list;

            // Act
            var result = _testEntity.SelectSubCountUAD(Guid.Empty, new EntityReporting(), new ClientConnections());

            // Assert
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void Get_States_And_Copies_ByIssueId_ReturnsSuccessResponse()
        {
            // Arrange
            ShimWorker.AllInstances.GetStateAndCopiesStringInt32ClientConnections = (a, b, c, d) => _dataTable;

            // Act
            var result = _testEntity.Get_States_And_Copies(
                Guid.Empty,
                SampleString,
                SampleId,
                new ClientConnections());

            // Assert
            VerifySuccessResponse(result, _dataTable);
        }
    }
}
