using System.Data.SqlClient;

using NUnit.Framework;
using Shouldly;

using UADShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;
using UASShimDataFunctions = FrameworkUAS.DataAccess.Fakes.ShimDataFunctions;
using KMCommonShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;

using UADReport = FrameworkUAD.DataAccess.Report;

namespace FrameworkUAD.UnitTests.DataAccess
{
    [TestFixture]
    public partial class ReportTest
    {
        private const string MethodSelectGeoBreakdownSingleCountryMvc = "SelectGeoBreakdown_Single_CountryMVC";
        private const string MethodSelectGeoBreakdownDomesticMvc = "SelectGeoBreakdown_DomesticMVC";
        private const string MethodGetFullSubscriberDetailsMvc = "GetFullSubscriberDetailsMVC";
        private const string MethodSelectCrossTabMvc = "SelectCrossTabMVC";
        private const string MethodSelectSubFieldsMVC = "SelectSubFieldsMVC";
        private const string MethodSelectSingleResponseMVC = "SelectSingleResponseMVC";

        [Test]
        public void SelectGeoBreakdown_Single_CountryMVC_SetParams_CmdParams()
        {
            // Arrange
            var sqlConnection = new SqlConnection();
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => sqlConnection;

            SqlCommand calledSqlCommand = null;
            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            _reportType.InvokeStatic(
                MethodSelectGeoBreakdownSingleCountryMvc,
                client,
                SampleFilterQuery,
                SampleIssueId,
                CountryIdSample);

            // Assert
            calledSqlCommand.ShouldNotBeNull();
            calledSqlCommand.ShouldSatisfyAllConditions(
                () => calledSqlCommand.CommandText.ShouldBe(UADReport.SpGeoSingleCountryMvc),
                () => calledSqlCommand.Connection.ShouldBeSameAs(sqlConnection));

            var parameters = calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(3),

                () => parameters[UADReport.ParamIssueId].Value.ShouldBe(SampleIssueId),
                () => parameters[UADReport.ParamQueries].Value.ShouldBe(SampleFilterQuery),
                () => parameters[UADReport.ParamCountryId].Value.ShouldBe(CountryIdSample));
        }

        [Test]
        public void SelectGeoBreakdown_DomesticMVC_SetParams_CmdParams()
        {
            // Arrange
            var sqlConnection = new SqlConnection();
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => sqlConnection;

            SqlCommand calledSqlCommand = null;
            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            _reportType.InvokeStatic(
                MethodSelectGeoBreakdownDomesticMvc,
                client,
                SampleFilterQuery,
                SampleIssueId,
                true);

            // Assert
            calledSqlCommand.ShouldNotBeNull();
            calledSqlCommand.ShouldSatisfyAllConditions(
                () => calledSqlCommand.CommandText.ShouldBe(UADReport.SpGeoBreakdownDomesticMvc),
                () => calledSqlCommand.Connection.ShouldBeSameAs(sqlConnection));

            var parameters = calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(3),

                () => parameters[UADReport.ParamIssueId].Value.ShouldBe(SampleIssueId),
                () => parameters[UADReport.ParamQueries].Value.ShouldBe(SampleFilterQuery),
                () => parameters[UADReport.ParamIncludeAddRemove].Value.ShouldBe(true));
        }

        [Test]
        public void GetFullSubscriberDetailsMVC_SetParams_CmdParams()
        {
            // Arrange
            var sqlConnection = new SqlConnection();
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => sqlConnection;

            SqlCommand calledSqlCommand = null;
            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            _reportType.InvokeStatic(
                MethodGetFullSubscriberDetailsMvc,
                client,
                SampleFilterQuery,
                SampleIssueId);

            // Assert
            calledSqlCommand.ShouldNotBeNull();
            calledSqlCommand.ShouldSatisfyAllConditions(
                () => calledSqlCommand.CommandText.ShouldBe(UADReport.SpPubSubscriptionsSelectAllFieldsWithFilterMvc),
                () => calledSqlCommand.Connection.ShouldBeSameAs(sqlConnection));

            var parameters = calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(2),

                () => parameters[UADReport.ParamIssueId].Value.ShouldBe(SampleIssueId),
                () => parameters[UADReport.ParamQueries].Value.ShouldBe(SampleFilterQuery));
        }

        [Test]
        public void SelectCrossTabMVC_SetParams_CmdParams()
        {
            // Arrange
            var sqlConnection = new SqlConnection();
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => sqlConnection;

            SqlCommand calledSqlCommand = null;
            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            _reportType.InvokeStatic(
                MethodSelectCrossTabMvc,
                client,
                SampleProductId,
                SampleRow,
                SampleCol,
                true,
                SampleFilterQuery,
                SampleIssueId,
                true);

            // Assert
            calledSqlCommand.ShouldNotBeNull();
            calledSqlCommand.ShouldSatisfyAllConditions(
                () => calledSqlCommand.CommandText.ShouldBe(UADReport.SpCrossTabMvc),
                () => calledSqlCommand.Connection.ShouldBeSameAs(sqlConnection));

            var parameters = calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(7),

                () => parameters[UADReport.ParamIssueId].Value.ShouldBe(SampleIssueId),
                () => parameters[UADReport.ParamProductId].Value.ShouldBe(SampleProductId),
                () => parameters[UADReport.ParamRow].Value.ShouldBe(SampleRow),
                () => parameters[UADReport.ParamCol].Value.ShouldBe(SampleCol),
                () => parameters[UADReport.ParamIncludeAddRemove].Value.ShouldBe(true),
                () => parameters[UADReport.ParamQueries].Value.ShouldBe(SampleFilterQuery),
                () => parameters[UADReport.ParamIncludeAddRemove].Value.ShouldBe(true));
        }

        [Test]
        public void SelectSubFieldsMVC_SetParams_CmdParams()
        {
            // Arrange
            var sqlConnection = new SqlConnection();
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => sqlConnection;

            SqlCommand calledSqlCommand = null;
            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            _reportType.InvokeStatic(
                MethodSelectSubFieldsMVC,
                client,
                SampleFilterQuery,
                SampleDemo,
                SampleIssueId);

            // Assert
            calledSqlCommand.ShouldNotBeNull();
            calledSqlCommand.ShouldSatisfyAllConditions(
                () => calledSqlCommand.CommandText.ShouldBe(UADReport.SpSubFieldsMvc),
                () => calledSqlCommand.Connection.ShouldBeSameAs(sqlConnection));

            var parameters = calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(3),

                () => parameters[UADReport.ParamQueries].Value.ShouldBe(SampleFilterQuery),
                () => parameters[UADReport.ParamDemo].Value.ShouldBe(SampleDemo),
                () => parameters[UADReport.ParamIssueId].Value.ShouldBe(SampleIssueId));
        }

        [Test]
        public void SelectSingleResponseMVC_SetParams_CmdParams()
        {
            // Arrange
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => _sqlConnection;

            SqlCommand calledSqlCommand = null;
            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            _reportType.InvokeStatic(
                MethodSelectSingleResponseMVC,
                client,
                SampleProductId,
                SampleRow,
                true,
                SampleFilterQuery,
                SampleIssueId);

            // Assert
            calledSqlCommand.ShouldNotBeNull();
            calledSqlCommand.ShouldSatisfyAllConditions(
                () => calledSqlCommand.CommandText.ShouldBe(UADReport.SpResponseMvc),
                () => calledSqlCommand.Connection.ShouldBeSameAs(_sqlConnection));

            var parameters = calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(5),

                () => parameters[UADReport.ParamPubId].Value.ShouldBe(SampleProductId),
                () => parameters[UADReport.ParamDemo].Value.ShouldBe(SampleRow),
                () => parameters[UADReport.ParamIncludeReportGroup].Value.ShouldBe(true),
                () => parameters[UADReport.ParamQueries].Value.ShouldBe(SampleFilterQuery),
                () => parameters[UADReport.ParamIssueId].Value.ShouldBe(SampleIssueId));
        }
    }
}
