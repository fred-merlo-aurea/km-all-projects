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
    public class GetDataTableTest : Fakes
    {
        private const string TableName = "spresult";

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
        public void GetDataTable_DbSet_ConnSelected(string dataBase, string connStr)
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

            ShimDbDataAdapter.AllInstances.FillDataSetString = (adapter, set, tableName) =>
            {
                var table = new DataTable(TableName);
                set.Tables.Add(table);
                return 0;
            };

            // Act
            var dataTable = DataFunctions.GetDataTable(dataBase, cmd);

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.TableName.ShouldBe(TableName);
            connection.ShouldNotBeNull();
            cmdConnectionStr.ShouldBe(connStr);
            _connOpened.ShouldBeTrue();
            _connClosed.ShouldBeTrue();
        }

        [Test]
        public void GetDataTableColumns_DataTableGiven_ColumnsReturned()
        {
            // Arrange
            var table = new DataTable(TableName);
            table.Columns.Add("Column1");
            table.Columns.Add("Column2");
            table.Columns.Add("Column3");

            // Act
            var columnList = DataFunctions.GetDataTableColumns(table);

            // Assert
            columnList.ShouldSatisfyAllConditions(
                () => columnList.Count.ShouldBe(3),
                () => columnList.Contains("Column1").ShouldBeTrue(),
                () => columnList.Contains("Column2").ShouldBeTrue(),
                () => columnList.Contains("Column3").ShouldBeTrue());
        }
    }
}
