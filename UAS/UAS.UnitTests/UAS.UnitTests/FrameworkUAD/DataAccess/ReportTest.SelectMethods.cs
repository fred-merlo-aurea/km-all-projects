using System.Collections.Generic;
using System.Data;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="Report"/>
    /// </summary>
    public partial class ReportTest
    {
        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForAddRemoves_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectForAddRemoves(Client, PubID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamPubId].Value.ShouldBe(PubID),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForAddRemoves),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectBPA_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectBPA(Client, new Reporting(), PrintColumns, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectBpa),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCategorySummary_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectCategorySummary(Client, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCategorySummary),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectQualificationBreakDown_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var rpt = new Reporting(typeof(Reporting).CreateInstance());
            var breakDownReports = new List<QualificationBreakdownReport>
            {
                typeof(QualificationBreakdownReport).CreateInstance()
            };

            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return breakDownReports.GetSqlDataReader();
            };

            // Act
            var result = Report.SelectQualificationBreakDown(Client, rpt, PrintColumns, true, Years);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(rpt.PublicationIDs),
                () => _sqlCommand.Parameters[ParamRegions].Value.ShouldBe(rpt.Regions),
                () => _sqlCommand.Parameters[ParamPrintColumn].Value.ShouldBe(PrintColumns),
                () => ((bool)_sqlCommand.Parameters[ParamDownload].Value).ShouldBeTrue(),
                () => _sqlCommand.Parameters[ParamYears].Value.ShouldBe(rpt.Year),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(breakDownReports.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectQualificationBreakDown),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCrossTab_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectCrossTab(Client, ProductID, Row, Col, true, Filters, AdHocFilters, IssueID, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamRow].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamCol].Value.ShouldBe(Col),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => ((bool)_sqlCommand.Parameters[ParamIncludeReportGroup].Value).ShouldBeTrue(),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCrossTab),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectDemoSubReport_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectDemoSubReport(Client, ProductID, Row, true, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamRow].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDemoSubReport),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdownInternational_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectGeoBreakdownInternational(Client, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownInternational),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdownSingleCountry_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectGeoBreakdown_Single_Country(Client, Filters, AdHocFilters, IssueID, CountryID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamCountryId].Value.ShouldBe(CountryID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownSingleCountry),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdownDomestic_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectGeoBreakdown_Domestic(Client, Filters, AdHocFilters, IssueID, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => ((bool)_sqlCommand.Parameters[ParamIncludeAddRemove].Value).ShouldBeTrue(),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownDomestic),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSingleResponse_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectSingleResponse(Client, ProductID, Row, true, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamPubId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParameterDemo].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamIncludeReportGroup].Value.ShouldBe(IncludeReportGroups),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSingleResponse),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectDemoXQualification_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectDemoXQualification(Client, ProductID, Row, Filters, AdHocFilters, IssueID, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamRow].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamIncludeReportGroup].Value.ShouldBe(IncludeReportGroups),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDemoXQualification),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdown_FCI_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var rpt = new Reporting();

            // Act
            var result = Report.SelectGeoBreakdown_FCI(Client, rpt, PrintColumns, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(rpt.PublicationIDs),
                () => _sqlCommand.Parameters[ParamRegions].Value.ShouldBe(rpt.Regions),
                () => ((bool)_sqlCommand.Parameters[ParamIncludeAllStates].Value).ShouldBeFalse(),
                () => _sqlCommand.Parameters[ParamWaveMail].Value.ShouldBe(rpt.WaveMail),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownDomestic),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectListReport_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var rpt = new Reporting();

            // Act
            var result = Report.SelectListReport(Client, rpt, ReportID, RowID, PrintColumns, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(rpt.PublicationIDs),
                () => _sqlCommand.Parameters[ParamRegions].Value.ShouldBe(rpt.Regions),
                () => _sqlCommand.Parameters[ParamReportId].Value.ShouldBe(ReportID),
                () => _sqlCommand.Parameters[ParameterRowId].Value.ShouldBe(RowID),
                () => _sqlCommand.Parameters[ParamWaveMail].Value.ShouldBe(rpt.WaveMail),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectListReport),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPar3c_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectPar3c(Client, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPar3C),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectQSourceBreakdown_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectQSourceBreakdown(Client, ProductID, true, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectQualificationBreakDown),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubFields_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectSubFields(Client, Filters, AdHocFilters, Demo, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParameterDemo].Value.ShouldBe(Demo),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubFields),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubsrc_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.SelectSubsrc(Client, Filters, AdHocFilters, true, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => ((bool)_sqlCommand.Parameters[ParamIncludeAddRemove].Value).ShouldBeTrue(),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubsrc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectAddRemove_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var rpt = new Reporting();

            // Act
            var result = Report.SelectAddRemove(Client, rpt, IssueID, PrintColumns, true);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(rpt.PublicationIDs),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamWaveMail].Value.ShouldBe(rpt.WaveMail),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectAddRemove),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubscriberCount_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var intList = new List<int> { 1, 2, 3 };
            var expectedList = new List<int> { 0, 0, 0 };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return intList.GetSqlDataReader();
            };

            // Act
            var result = Report.SelectSubscriberCount(Xml, AdHocXml, true, true, IssueID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilterString].Value.ShouldBe(Xml),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParameterAdHocXml].Value.ShouldBe(AdHocXml),
                () => ((bool)_sqlCommand.Parameters[ParamUserArchive].Value).ShouldBeTrue(),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(expectedList),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubscriberCount),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubscriberCopies_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var rpt = new Reporting();
            var expectedList = new List<int> { 0, 0, 0 };
            var intList = new List<int> { 1, 2, 3 };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return intList.GetSqlDataReader();
            };

            // Act
            var result = Report.SelectSubscriberCopies(rpt, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamPublicationId].Value.ShouldBe(rpt.PublicationIDs),
                () => _sqlCommand.Parameters[ParamWaveMail].Value.ShouldBe(rpt.WaveMail),
                () => result.ShouldBe(expectedList),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubscriberCopies),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubCountUAD_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var rpt = new Reporting();
            var expectedList = new List<int> { 0, 0, 0 };
            var intList = new List<int> { 1, 2, 3 };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return intList.GetSqlDataReader();
            };

            // Act
            var result = Report.SelectSubCountUAD(rpt, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamPubIds].Value.ShouldBe(rpt.PublicationIDs),
                () => _sqlCommand.Parameters[ParamRegions].Value.ShouldBe(rpt.Regions),
                () => _sqlCommand.Parameters[ParamDemo31].Value.ShouldBe(rpt.Demo31),
                () => _sqlCommand.Parameters[ParamDemo32].Value.ShouldBe(rpt.Demo32),
                () => _sqlCommand.Parameters[ParamDemo33].Value.ShouldBe(rpt.Demo33),
                () => _sqlCommand.Parameters[ParamDemo34].Value.ShouldBe(rpt.Demo34),
                () => _sqlCommand.Parameters[ParamDemo35].Value.ShouldBe(rpt.Demo35),
                () => _sqlCommand.Parameters[ParamDemo36].Value.ShouldBe(rpt.Demo36),
                () => _sqlCommand.Parameters[ParamUadResponseIds].Value.ShouldBe(rpt.UADResponseIDs),
                () => _sqlCommand.Parameters[ParamIsMailable].Value.ShouldBe(rpt.IsMailable),
                () => _sqlCommand.Parameters[ParamEmailStatusId].Value.ShouldBe(rpt.EmailStatusIDs),
                () => _sqlCommand.Parameters[ParamOpenSearchType].Value.ShouldBe(rpt.OpenSearchType),
                () => _sqlCommand.Parameters[ParamOpenCount].Value.ShouldBe(rpt.OpenCount),
                () => _sqlCommand.Parameters[ParamOpenDateFrom].Value.ShouldBe(rpt.OpenDateFrom),
                () => _sqlCommand.Parameters[ParamOpenDateTo].Value.ShouldBe(rpt.OpenDateTo),
                () => _sqlCommand.Parameters[ParamOpenBlastId].Value.ShouldBe(rpt.OpenBlastID),
                () => _sqlCommand.Parameters[ParamOpenEmailSubject].Value.ShouldBe(rpt.OpenEmailSubject),
                () => _sqlCommand.Parameters[ParamOpenEmailFromDate].Value.ShouldBe(rpt.OpenEmailFromDate),
                () => _sqlCommand.Parameters[ParamOpenEmailToDate].Value.ShouldBe(rpt.OpenEmailToDate),
                () => _sqlCommand.Parameters[ParamClickSearchType].Value.ShouldBe(rpt.ClickSearchType),
                () => _sqlCommand.Parameters[ParamClickCount].Value.ShouldBe(rpt.ClickCount),
                () => _sqlCommand.Parameters[ParamClickUrl].Value.ShouldBe(rpt.ClickURL),
                () => _sqlCommand.Parameters[ParamClickDateFrom].Value.ShouldBe(rpt.ClickDateFrom),
                () => _sqlCommand.Parameters[ParamClickDateTo].Value.ShouldBe(rpt.ClickDateTo),
                () => _sqlCommand.Parameters[ParamClickBlastId].Value.ShouldBe(rpt.ClickBlastID),
                () => _sqlCommand.Parameters[ParamClickEmailSubject].Value.ShouldBe(rpt.ClickEmailSubject),
                () => _sqlCommand.Parameters[ParamClickEmailFromDate].Value.ShouldBe(rpt.ClickEmailFromDate),
                () => _sqlCommand.Parameters[ParamClickEmailToDate].Value.ShouldBe(rpt.ClickEmailToDate),
                () => _sqlCommand.Parameters[ParamDomain].Value.ShouldBe(rpt.Domain),
                () => _sqlCommand.Parameters[ParamVisitUrl].Value.ShouldBe(rpt.VisitsURL),
                () => _sqlCommand.Parameters[ParamVisitsDateFrom].Value.ShouldBe(rpt.VisitsDateFrom),
                () => _sqlCommand.Parameters[ParamVisitsDateTo].Value.ShouldBe(rpt.VisitsDateTo),
                () => _sqlCommand.Parameters[ParamBrandId].Value.ShouldBe(rpt.BrandID),
                () => _sqlCommand.Parameters[ParamSearchType].Value.ShouldBe(rpt.SearchType),
                () => _sqlCommand.Parameters[RangeMaxLatMin].Value.ShouldBe(rpt.RangeMaxLatMin),
                () => _sqlCommand.Parameters[RangeMaxLatMax].Value.ShouldBe(rpt.RangeMaxLatMax),
                () => _sqlCommand.Parameters[RangeMaxLonMin].Value.ShouldBe(rpt.RangeMaxLonMin),
                () => _sqlCommand.Parameters[RangeMaxLonMax].Value.ShouldBe(rpt.RangeMaxLonMax),
                () => _sqlCommand.Parameters[RangeMinLatMin].Value.ShouldBe(rpt.RangeMinLatMin),
                () => _sqlCommand.Parameters[RangeMinLatMax].Value.ShouldBe(rpt.RangeMinLatMax),
                () => _sqlCommand.Parameters[RangeMinLonMin].Value.ShouldBe(rpt.RangeMinLonMin),
                () => _sqlCommand.Parameters[RangeMinLonMax].Value.ShouldBe(rpt.RangeMinLonMax),
                () => result.ShouldBe(expectedList),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubCountUad),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectActiveIssueSplitsCounts_WhenCalled_VerifySqlParameters()
        {
            Counts count = typeof(Counts).CreateInstance();
            var counts = new List<Counts>
            {
                count
            };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return counts.GetSqlDataReader();
            };
            // Act
            var result = Report.SelectActiveIssueSplitsCounts(ProductID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectActiveIssueSplitsCounts),
                () => result.ShouldBe(count),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}