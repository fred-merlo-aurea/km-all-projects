using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.common.classes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Common.DataFunctions_cs
{
    [TestFixture]
    public class ExecuteScalarTest : Fakes
    {
        private const string SqlCommandText = "select 1";

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
        public void ExecuteScalarCmd_DbSet_ConnSelected(string dataBase, string connStr)
        {
            // Arrange
            SqlConnection connection = null;
            var executeScalarCalled = false;
            var cmdShim = new ShimSqlCommand();
            cmdShim.ConnectionGet = () => { return connection; };
            string cmdConnectionStr = null;
            cmdShim.ConnectionSetSqlConnection = conn =>
            {
                cmdConnectionStr = conn.ConnectionString;
                connection = conn;
            };
            var scalar = new object();
            cmdShim.ExecuteScalar = () =>
            {
                executeScalarCalled = true;
                return scalar;
            };
            SqlCommand cmd = cmdShim;

            // Act
            var result = DataFunctions.ExecuteScalar(dataBase, cmd);

            // Assert
            result.ShouldBeSameAs(scalar);
            connection.ShouldNotBeNull();
            cmdConnectionStr.ShouldBe(connStr);
            _connOpened.ShouldBeTrue();
            _connClosed.ShouldBeTrue();
            executeScalarCalled.ShouldBeTrue();
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
        public void ExecuteScalarStr_DbSet_ConnSelected(string dataBase, string connStr)
        {
            // Arrange
            SqlConnection connection = null;
            var executeScalarCalled = false;
            var cmdShim = new ShimSqlCommand();
            cmdShim.ConnectionGet = () => { return connection; };
            string cmdConnectionStr = null;

            var scalar = new object();
            ShimSqlCommand.AllInstances.ExecuteScalar = command =>
            {
                executeScalarCalled = true;
                return scalar;
            };

            ShimSqlCommand.ConstructorStringSqlConnection = (command, sql, conn) =>
            {
                cmdConnectionStr = conn.ConnectionString;
                connection = conn;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = (command) => { return connection; };

            // Act
            var result = DataFunctions.ExecuteScalar(dataBase, SqlCommandText);

            // Assert
            result.ShouldBeSameAs(scalar);
            connection.ShouldNotBeNull();
            cmdConnectionStr.ShouldBe(connStr);
            _connOpened.ShouldBeTrue();
            _connClosed.ShouldBeTrue();
            executeScalarCalled.ShouldBeTrue();
        }
    }
}
