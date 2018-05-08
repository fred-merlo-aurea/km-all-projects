using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.common.classes;
using ECN.Common.Helpers;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Common.DataFunctions_cs
{
    [TestFixture]
    public class ExecuteTest: Fakes
    {
        private const string SqlSample = "SELECT 1";

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
        public void Execute_DbSet_ConnSelected(string dataBase, string connStr)
        {
            // Arrange
            SqlConnection connection = null;
            var executeNonQueryCalled = false;
            var cmdShim = new ShimSqlCommand();
            cmdShim.ConnectionGet = () => { return connection; };
            string cmdConnectionStr = null;
            cmdShim.ConnectionSetSqlConnection = conn =>
            {
                cmdConnectionStr = conn.ConnectionString;
                connection = conn;
            };

            cmdShim.ExecuteNonQuery = () =>
            {
                executeNonQueryCalled = true;
                return SqlSuccessCode;
            };
            SqlCommand cmd = cmdShim;

            // Act
            var result = DataFunctions.Execute(dataBase, cmd);

            // Assert
            result.ShouldBe(SqlSuccessCode);
            connection.ShouldNotBeNull();
            cmdConnectionStr.ShouldBe(connStr);
            _connOpened.ShouldBeTrue();
            _connClosed.ShouldBeTrue();
            executeNonQueryCalled.ShouldBeTrue();
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
        public void Execute_DbNameQuery_ConnSelected(string dataBase, string connStr)
        {
            // Arrange
            SqlConnection connection = null;
            var executeNonQueryCalled = false;
            string cmdConnectionStr = null;

            ShimSqlCommand.AllInstances.ConnectionGet = command => connection;
            ShimSqlCommand.AllInstances.ConnectionSetSqlConnection = (command, sqlConnection) =>
            {
                cmdConnectionStr = sqlConnection.ConnectionString;
                connection = sqlConnection;
            };

            var calledSqlQuery = string.Empty;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                calledSqlQuery = command.CommandText;
                executeNonQueryCalled = true;
                return SqlSuccessCode;
            };

            new DataFunctions();

            // Act
            var result = DataFunctions.Execute(dataBase, SqlSample);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(SqlSuccessCode),
                () => connection.ShouldNotBeNull(),
                () => cmdConnectionStr.ShouldBe(connStr),
                () => _connOpened.ShouldBeTrue(),
                () => _connClosed.ShouldBeTrue(),
                () => executeNonQueryCalled.ShouldBeTrue(),
                () => calledSqlQuery.ShouldBe(SqlSample)
            );
        }

        [TestCase(ConnStrConnStr)]
        public void Execute_Query_ConnSelected(string connStr)
        {
            // Arrange
            SqlConnection connection = null;
            var executeNonQueryCalled = false;
            string cmdConnectionStr = null;

            ShimSqlCommand.AllInstances.ConnectionGet = command => connection;
            ShimSqlCommand.AllInstances.ConnectionSetSqlConnection = (command, sqlConnection) =>
            {
                cmdConnectionStr = sqlConnection.ConnectionString;
                connection = sqlConnection;
            };

            var calledSqlQuery = string.Empty;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                calledSqlQuery = command.CommandText;
                executeNonQueryCalled = true;
                return SqlSuccessCode;
            };

            new DataFunctions();

            // Act
            var result = DataFunctions.Execute(SqlSample);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(SqlSuccessCode),
                () => connection.ShouldNotBeNull(),
                () => cmdConnectionStr.ShouldBe(connStr),
                () => _connOpened.ShouldBeTrue(),
                () => _connClosed.ShouldBeTrue(),
                () => executeNonQueryCalled.ShouldBeTrue(),
                () => calledSqlQuery.ShouldBe(SqlSample)
            );
        }
    }
}
