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
        [Test]
        public void SelectListReport_SetParams_CmdParams()
        {
            // Arrange
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => _sqlConnection;

            KMCommonShimDataFunctions.GetDataTableViaAdapterSqlCommand = command =>
            {
                _calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            var printColumns = PrintColumnsSample;
            var reportId = ReportIdSample;
            var rowId = RowIdSample;
            var download = true;

            // Act
            UADReport.SelectListReport(
                client,
                Reporting,
                reportId,
                rowId,
                printColumns,
                download);

            // Assert
            _calledSqlCommand.ShouldNotBeNull();
            _calledSqlCommand.ShouldSatisfyAllConditions(
                () => _calledSqlCommand.CommandText.ShouldBe(UADReport.SpViewResponseTotals),
                () => _calledSqlCommand.Connection.ShouldBeSameAs(_sqlConnection));

            var parameters = _calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(22),

                () => AssertCommonFields(parameters),

                () => parameters[UADReport.ParamProductId].Value.ShouldBe(Reporting.PublicationIDs),
                () => parameters[UADReport.ParamRegions].Value.ShouldBe(Reporting.Regions),
                () => parameters[UADReport.ParamMobile].Value.ShouldBe(Reporting.Mobile),
                () => parameters[UADReport.ParamYear].Value.ShouldBe(Reporting.Year),
                () => parameters[UADReport.ParamStartDate].Value.ShouldBe(Reporting.FromDate),
                () => parameters[UADReport.ParamEndDate].Value.ShouldBe(Reporting.ToDate),
                () => parameters[UADReport.ParamTransactionCodes].Value.ShouldBe(Reporting.TransactionCodes),

                () => parameters[UADReport.ParamReportId].Value.ShouldBe(reportId),
                () => parameters[UADReport.ParamRowId].Value.ShouldBe(rowId),
                () => parameters[UADReport.ParamWaveMail].Value.ShouldBe(Reporting.WaveMail));
        }
    }
}
