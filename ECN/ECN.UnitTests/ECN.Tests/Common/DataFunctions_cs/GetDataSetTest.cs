using System.Data;
using System.Data.Common.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;

using NUnit.Framework;
using Shouldly;

using ecn.common.classes;

namespace ECN.Tests.Common.DataFunctions_cs
{
    [TestFixture]
    public class GetDataSet : Fakes
    {
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [TearDown]
        public void Teardown()
        {
            ReleaseFakes();
        }

        [TestCase(null, ConnStrConnStr)]
        [TestCase(DbAccounts, ConnStrConAccounts)]
        [TestCase(DbCollector, ConnStrConCollector)]
        [TestCase(DbCreator, ConnStrConCreator)]
        [TestCase(DbCommunicator, ConnStrConCommunicator)]
        [TestCase(DbCharity, ConnStrConCharity)]
        [TestCase(DbPublisher, ConnStrConPublisher)]
        [TestCase(DbMisc, ConnStrConMisc)]
        [TestCase(DbActivity, ConnStrConActivity)]
        public void GetDataSet_DbSet_ConnSelected(string dataBase, string connStr)
        {
            // Arrange
            SqlConnection connection = null;
            var cmdShim = new ShimSqlCommand();
            cmdShim.ConnectionGet = () => { return connection; };
            string cmdConnectionStr = null;
            cmdShim.ConnectionSetSqlConnection = conn =>
            {
                cmdConnectionStr = conn.ConnectionString;
                connection = conn;
            };

            SqlCommand cmd = cmdShim;

            ShimDbDataAdapter.AllInstances.FillDataSet = (adapter, set) =>
            {
                var table = new DataTable(TableName);
                set.Tables.Add(table);
                return 0;
            };

            // Act
            var dataSet = DataFunctions.GetDataSet(dataBase, cmd);

            // Assert
            dataSet.ShouldNotBeNull();
            dataSet.Tables.Count.ShouldBe(1);
            var dataTable = dataSet.Tables[0];
            dataTable.ShouldNotBeNull();
            dataTable.TableName.ShouldBe(TableName);
            connection.ShouldNotBeNull();
            cmdConnectionStr.ShouldBe(connStr);
            _connOpened.ShouldBeTrue();
            _connClosed.ShouldBeTrue();
        }
    }
}
