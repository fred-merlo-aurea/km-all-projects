using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public partial class ReportTest
    {
        private const string MethodGet = "Get";
        private const string MethodGetList = "GetList";
        private const string MethodGetCategoryList = "GetCategoryList";
        private const string MethodGetSubscriberResponseDetailsMvc = "GetSubscriberResponseDetailsMVC";
        private const string MethodGetSubscriberPaidDetailsMvc = "GetSubscriberPaidDetailsMVC";
        private const string MethodGetSubscriberDetailsMvc = "GetSubscriberDetailsMVC";
        private const string MethodGetFullSubscriberDetailsMvc = "GetFullSubscriberDetailsMVC";

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodGet, new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodGetList, new object[] { new SqlCommand() }) as List<Entity.Report>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void GetCategoryList_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var categorySummaryReports = new List<CategorySummaryReport>
            {
                typeof(CategorySummaryReport).CreateInstance()
            };

            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return categorySummaryReports.GetSqlDataReader();
            };

            // Act
            var result =
                typeof(Report).CallMethod(MethodGetCategoryList, new object[] { new SqlCommand() }) as List<CategorySummaryReport>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result
                    .IsListContentMatched(categorySummaryReports.ToList())
                    .ShouldBeTrue());
        }

        [Test]
        public void GetSubscriberResponseDetailsMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodGetSubscriberResponseDetailsMvc, new object[] { Client, FilterQuery, IssueID });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetSubscriberResponseDetailsMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetSubscriberPaidDetailsMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodGetSubscriberPaidDetailsMvc, new object[] { Client, FilterQuery });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetSubscriberPaidDetailsMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetSubscriberDetailsMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result =
                typeof(Report).CallMethod(MethodGetSubscriberDetailsMvc, new object[] { Client, FilterQuery, IssueID });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetSubscriberDetailsMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetFullSubscriberDetailsMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = typeof(Report).CallMethod(MethodGetFullSubscriberDetailsMvc, new object[] { Client, FilterQuery, IssueID });

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetFullSubscriberDetailsMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetResponses_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetResponses(Client, ProductID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetResponses),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetProfileFields_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetProfileFields(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetProfileFields),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetIssueDates_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetIssueDates(Client, ProductID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetIssueDates),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_Countries_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.Get_Countries(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCountries),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void ReqFlagSummary_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.ReqFlagSummary(Client, ProductID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamProductId].Value.ShouldBe(ProductID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcReqFlagSummary),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetSubscriberDetails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetSubscriberDetails(Client, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetSubscriberDetails),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetFullSubscriberDetails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetFullSubscriberDetails(Client, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetFullSubscriberDetails),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetSubscriberPaidDetails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetSubscriberPaidDetails(Client, Filters, AdHocFilters);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetSubscriberPaidDetailsMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetSubscriberResponseDetails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetSubscriberResponseDetails(Client, Filters, AdHocFilters, IssueID);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamAdhocFilters].Value.ShouldBe(AdHocFilters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetSubscriberResponseDetails),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetStateAndCopies_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetStateAndCopies(Filters, IssueID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetStateAndCopies),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetStateAndCopiesMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetStateAndCopiesMVC(FilterQuery, IssueID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetStateAndCopiesMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCountryAndCopies_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetCountryAndCopies(Filters, IssueID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamFilters].Value.ShouldBe(Filters),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCountryAndCopies),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void GetCountryAndCopiesMVC_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Report.GetCountryAndCopiesMVC(FilterQuery, IssueID, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters[ParamQueries].Value.ShouldBe(FilterQuery),
                () => _sqlCommand.Parameters[ParamIssueId].Value.ShouldBe(IssueID),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcGetCountryAndCopiesMvc),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}