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
        public void SelectSubCountUAD_SetParams_CmdParams()
        {
            // Arrange
            ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => _sqlConnection;

            ShimReport.GetIntListSqlCommand = command =>
            {
                _calledSqlCommand = command;
                return null;
            };

            var client = new KMPlatform.Object.ClientConnections();

            // Act
            UADReport.SelectSubCountUAD(Reporting, client);

            // Assert
            _calledSqlCommand.ShouldNotBeNull();
            _calledSqlCommand.ShouldSatisfyAllConditions(
                () => _calledSqlCommand.CommandText.ShouldBe(UADReport.SpGetSubscriptionIdsFromFilterUad),
                () => _calledSqlCommand.Connection.ShouldBeSameAs(_sqlConnection));

            var parameters = _calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(54),

                () => AssertCommonFields(parameters),

                () => parameters[UADReport.ParamPubIds].Value.ShouldBe(Reporting.PublicationIDs));
        }
    }
}
