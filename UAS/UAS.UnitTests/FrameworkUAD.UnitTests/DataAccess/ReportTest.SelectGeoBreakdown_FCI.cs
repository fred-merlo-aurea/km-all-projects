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
        public void SelectGeoBreakdown_FCI_SetParams_CmdParams()
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
            var download = true;

            // Act
            UADReport.SelectGeoBreakdown_FCI(client, Reporting, printColumns, download);

            // Assert
            _calledSqlCommand.ShouldNotBeNull();
            _calledSqlCommand.ShouldSatisfyAllConditions(
                () => _calledSqlCommand.CommandText.ShouldBe( UADReport.SpGeoBreakdownDomestic),
                () => _calledSqlCommand.Connection.ShouldBeSameAs(_sqlConnection));

            var parameters = _calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(21),

                () => AssertCommonFields(parameters),

                () => parameters[UADReport.ParamProductId].Value.ShouldBe(Reporting.PublicationIDs),
                () => parameters[UADReport.ParamRegions].Value.ShouldBe(Reporting.Regions),
                () => parameters[UADReport.ParamMobile].Value.ShouldBe(Reporting.Mobile),
                () => parameters[UADReport.ParamFax].Value.ShouldBe(Reporting.Fax),
                () => parameters[UADReport.ParamYear].Value.ShouldBe(Reporting.Year),
                () => parameters[UADReport.ParamStartDate].Value.ShouldBe(Reporting.FromDate),
                () => parameters[UADReport.ParamEndDate].Value.ShouldBe(Reporting.ToDate),
                () => parameters[UADReport.ParamTransactionCodes].Value.ShouldBe(Reporting.TransactionCodes),
                () => parameters[UADReport.ParamIncludeAllStates].Value.ShouldBe(false),
                () => parameters[UADReport.ParamWaveMail].Value.ShouldBe(Reporting.WaveMail));
        }
    }
}
