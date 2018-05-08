using System.Collections.Generic;
using System.Data;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class ReportTest
    {
        private const string MethodSelectDemoSubReportMvc = "SelectDemoSubReportMVC";
        private const string MethodSelectCategorySummaryMvc = "SelectCategorySummaryMVC";
        private const string MethodSelectDemoXQualificationMvc = "SelectDemoXQualificationMVC";
        private const string MethodSelectGeoBreakDownInternationalMvc = "SelectGeoBreakdownInternationalMVC";
        private const string MethodSelectPar3CMvc = "SelectPar3cMVC";
        private const string MethodSelectQSourceBreakdownMvc = "SelectQSourceBreakdownMVC";
        private const string MethodSelectSubrcMvc = "SelectSubsrcMVC";
        private const string MethodSelectSubFieldMvc = "SelectSubFieldsMVC";
        private const string MethodSelectSingleResponsesMvc = "SelectSingleResponseMVC";
        private const string MethodSelectGeoBreakDownSingleCountryMvc = "SelectGeoBreakdown_Single_CountryMVC";
        private const string MethodSelectCrossTabMvc = "SelectCrossTabMVC";

        [Test]
        public void SelectDemoSubReportMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectDemoSubReportMvc,
                new object[] {Client, ProductID, Row, IncludeAddRemove, FilterQuery, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamRow].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDemoSubReportMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCategorySummaryMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectCategorySummaryMvc,
                new object[] {Client, FilterQuery, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCategorySummaryMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectDemoXQualificationMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectDemoXQualificationMvc,
                new object[] {Client, ProductID, Row, FilterQury, IssueID, true});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQury),
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamRow].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamIncludeReportGroup].Value.ShouldBe(IncludeReportGroups),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectDemoXQualificationMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdownInternationalMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectGeoBreakDownInternationalMvc,
                new object[] {Client, FiltersQuery, IssueID, ProductID, true});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FiltersQuery),
                () => ((bool) _sqlCommand.Parameters[ParamIncludeCustomRegion].Value).ShouldBeTrue(),
                () => _sqlCommand.Parameters[ParamPubId].Value.ShouldBe(ProductID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownInternationalMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPar3cMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectPar3CMvc, new object[] {Client, FilterQuery, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPar3CMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectQSourceBreakdownMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectQSourceBreakdownMvc,
                new object[] {Client, ProductID, true, FilterQuery, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectQSourceBreakdownMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubsrcMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result =
                typeof(Report).CallMethod(MethodSelectSubrcMvc, new object[] {Client, FilterQuery, true, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubsrcMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubFieldsMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectSubFieldMvc,
                new object[] {Client, FilterQuery, Demo, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubFieldsMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSingleResponseMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectSingleResponsesMvc,
                new object[] {Client, ProductID, Row, true, FilterQuery, IssueID});

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIncludeReportGroup].Value.ShouldBe(IncludeReportGroups),
                () => _sqlCommand.Parameters[ParamPubId].Value.ShouldBe(ProductID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSingleResponseMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdown_Single_CountryMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectGeoBreakDownSingleCountryMvc, new object[] { Client, FilterQuery, IssueID, CountryID });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamCountryId].Value.ShouldBe(CountryID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownSingleCountryMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectGeoBreakdown_DomesticMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(ParamSelectGeoBreakDownDomesticMvc, new object[] { Client, FilterQuery, IssueID, true });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectGeoBreakdownDomesticMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectCrossTabMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodSelectCrossTabMvc, new object[] { Client, ProductID, Row, Col, true, FilterQuery, IssueID, true });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => _sqlCommand.Parameters[ParamRow].Value.ShouldBe(Row),
                () => _sqlCommand.Parameters[ParamCol].Value.ShouldBe(Col),
                () => _sqlCommand.Parameters[ParamIncludeAddRemove].Value.ShouldBe(IncludeAddRemove),
                () => _sqlCommand.Parameters[ParamIncludeReportGroup].Value.ShouldBe(IncludeReportGroups),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectCrossTabMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectSubscriberCountMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var expectedList = new List<int> { 0, 0, 0 };
            var intList = new List<int> { 1, 2, 3 };
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return intList.GetSqlDataReader();
            };

            // Act
            var result = Report.SelectSubscriberCountMVC(FilterQuery, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => result.ShouldBe(expectedList),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectSubscriberCountMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}