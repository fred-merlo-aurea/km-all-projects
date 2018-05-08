using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using ECN.Common.Interfaces;
using Moq;
using NUnit.Framework;

namespace ECN.Tests
{
    [TestFixture]
    public class DataFunctionsTest
    {
        private string _openConnectionString;
        private string _closeConnectionString;
        private string _executeConnectionString;
        Mock<IDatabaseFunctions> _databaseFunctions;

        [SetUp]
        public void SetUp()
        {
            _openConnectionString = string.Empty;
            _closeConnectionString = string.Empty;
            _executeConnectionString = string.Empty;
            _databaseFunctions = new Mock<IDatabaseFunctions>();
            _databaseFunctions.Setup(x => x.OpenConnection(It.IsAny<IDbCommand>()))
                .Callback<IDbCommand>(r => _openConnectionString = r.Connection.ConnectionString);
            _databaseFunctions.Setup(x => x.CloseConnection(It.IsAny<IDbConnection>()))
                .Callback<IDbConnection>(r => _closeConnectionString = r.ConnectionString);
        }

        [Test]
        public void Execute_AccountsDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "act";
            var databaseName = "accounts";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_CollectorDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "col";
            var databaseName = "collector";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_CreatorDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "cre";
            var databaseName = "creator";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_CommunicatorDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "com";
            var databaseName = "communicator";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_CharityDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "chr";
            var databaseName = "charity";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_PublisherDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "pub";
            var databaseName = "publisher";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_MiscDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "misc";
            var databaseName = "misc";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_ActivityDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "activity";
            var databaseName = "activity";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        [Test]
        public void Execute_DefaultDB_SetsConnectionStringToAct()
        {
            // Arrange
            var databaseKey = "connString";
            var databaseName = "default";
            var connectionString = ConfigurationManager.AppSettings[databaseKey];
            var runStaticConstructor = new DataFunctions(_databaseFunctions.Object);
            SetupExecuteNonQuery();

            // Act
            DataFunctions.Execute(databaseName, string.Empty);

            // Assert
            AssertConnectionStrings(connectionString);
        }

        private void SetupExecuteNonQuery()
        {
            _databaseFunctions.Setup(x => x.ExecuteNonQuery(It.IsAny<IDbCommand>()))
                .Callback<IDbCommand>(r => _executeConnectionString = r.Connection.ConnectionString);
        }
        private void SetupExecuteScalar()
        {
            _databaseFunctions.Setup(x => x.ExecuteScalar(It.IsAny<IDbCommand>()))
                .Callback<IDbCommand>(r => _executeConnectionString = r.Connection.ConnectionString);
        }

        private void AssertConnectionStrings(string connectionString)
        {
            Assert.That(_openConnectionString, Is.EqualTo(connectionString));
            Assert.That(_closeConnectionString, Is.EqualTo(connectionString));
            Assert.That(_executeConnectionString, Is.EqualTo(connectionString));
        }
    }
}
