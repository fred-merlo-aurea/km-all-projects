using System.Data.SqlClient;

using FrameworkUAD.DataAccess.Fakes;
using NUnit.Framework;
using Shouldly;

using UADReport = FrameworkUAD.DataAccess.Report;

namespace FrameworkUAD.UnitTests.DataAccess
{
    [TestFixture]
    public partial class ReportTest
    {
        [Test]
        public void SelectQualificationBreakDown_SetParams_CmdParams()
        {
            // Arrange
            ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => _sqlConnection;

            ShimReport.GetQualificationListSqlCommand = command =>
            {
                _calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            var printColumns = PrintColumnsSample;
            var download = true;

            // Act
            UADReport.SelectQualificationBreakDown(
                client,
                Reporting,
                printColumns,
                download, 
                2018);

            // Assert
            _calledSqlCommand.ShouldNotBeNull();
            _calledSqlCommand.ShouldSatisfyAllConditions(
                () => _calledSqlCommand.CommandText.ShouldBe(UADReport.SpQualificationBreakdown),
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
                () => parameters[UADReport.ParamPrintColumns].Value.ShouldBe(printColumns),
                () => parameters[UADReport.ParamDownload].Value.ShouldBe(download),
                () => parameters[UADReport.ParamYears].Value.ShouldBe(Reporting.Year));
        }
    }
}
