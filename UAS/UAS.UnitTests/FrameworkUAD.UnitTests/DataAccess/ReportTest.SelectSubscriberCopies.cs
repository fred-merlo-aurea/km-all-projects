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
        public void SelectSubscriberCopies_SetParams_CmdParams()
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
            UADReport.SelectSubscriberCopies(Reporting, client);

            // Assert
            _calledSqlCommand.ShouldNotBeNull();
            _calledSqlCommand.ShouldSatisfyAllConditions(
                () => _calledSqlCommand.CommandText.ShouldBe(UADReport.SpGetSubscriptionIdsCopiesFromFilter),
                () => _calledSqlCommand.Connection.ShouldBeSameAs(_sqlConnection));

            var parameters = _calledSqlCommand.Parameters;
            parameters.ShouldNotBeNull();
            parameters.ShouldSatisfyAllConditions(
                () => parameters.Count.ShouldBe(19),

                () => AssertCommonFields(parameters),

                () => parameters[UADReport.ParamPublicationId].Value.ShouldBe(Reporting.PublicationIDs),
                () => parameters[UADReport.ParamMobile].Value.ShouldBe(Reporting.Mobile),
                () => parameters[UADReport.ParamYear].Value.ShouldBe(Reporting.Year),
                () => parameters[UADReport.ParamStartDate].Value.ShouldBe(Reporting.FromDate),
                () => parameters[UADReport.ParamEndDate].Value.ShouldBe(Reporting.ToDate),
                () => parameters[UADReport.ParamTransactionCodes].Value.ShouldBe(Reporting.TransactionCodes),
                () => parameters[UADReport.ParamWaveMail].Value.ShouldBe(Reporting.WaveMail));
        }
    }
}
