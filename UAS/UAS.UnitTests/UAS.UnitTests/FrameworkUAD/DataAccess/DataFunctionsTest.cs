using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using FrameworkUAD.DataAccess;
using KMPlatform.Entity;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using Microsoft.QualityTools.Testing.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    public class DataFunctionsTest 
    {
        private const string AppSettingsIsDemoKey = "IsDemo";
        private const string AppSettingsIsNetworkDeployedKey = "IsNetworkDeployed";
        private const string DemoDbConnecionString = "1.0.216.17";
        private const string DemoDbConnecionStringDeployed = "1.0.10.10";
        private const string LiveDbConnecionString = "2.0.216.17";
        private const string LiveDbConnecionStringDeployed = "2.0.10.10";

        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DataFunctionsTest_GetClientSqlConnectionForClientDeployed(bool isDemo)
        {
            string actualConnectionString = null;

            // Arrange
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                { AppSettingsIsDemoKey, isDemo.ToString() },
                { AppSettingsIsNetworkDeployedKey, true.ToString() } 
            };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
                    {
                        actualConnectionString = connectionString;
                    };

            var expectedConnectionString = isDemo ? DemoDbConnecionStringDeployed : LiveDbConnecionStringDeployed;

            var client = new Client
            {
                ClientLiveDBConnectionString = LiveDbConnecionStringDeployed,
                ClientTestDBConnectionString = DemoDbConnecionStringDeployed
            };

            // Act
            var sqlConnection = DataFunctions.GetClientSqlConnection(client);

            // Assert
            sqlConnection.ShouldNotBeNull();
            actualConnectionString.ShouldBe(expectedConnectionString);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DataFunctionsTest_GetClientSqlConnectionForClientNotDeployed(bool isDemo)
        {
            string actualConnectionString = null;

            // Arrange
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                { AppSettingsIsDemoKey, isDemo.ToString() },
                { AppSettingsIsNetworkDeployedKey, false.ToString() } 
            };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
                {
                    actualConnectionString = connectionString;
                };

            var expectedConnectionString = isDemo ? DemoDbConnecionString : LiveDbConnecionString;

            var client = new Client
            {
                ClientLiveDBConnectionString = LiveDbConnecionString,
                ClientTestDBConnectionString = DemoDbConnecionString
            };

            // Act
            var sqlConnection = DataFunctions.GetClientSqlConnection(client);

            // Assert
            sqlConnection.ShouldNotBeNull();
            actualConnectionString.ShouldBe(expectedConnectionString);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DataFunctionsTest_GetClientSqlConnectionForClientConnectionsDeployed(bool isDemo)
        {
            string actualConnectionString = null;

            // Arrange
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                { AppSettingsIsDemoKey, isDemo.ToString() },
                { AppSettingsIsNetworkDeployedKey, true.ToString() }
            };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
                {
                    actualConnectionString = connectionString;
                };

            var expectedConnectionString = isDemo ? DemoDbConnecionStringDeployed : LiveDbConnecionStringDeployed;

            var client = new ClientConnections()
                             {
                                 ClientLiveDBConnectionString = LiveDbConnecionStringDeployed,
                                 ClientTestDBConnectionString = DemoDbConnecionStringDeployed
                             };

            // Act
            var sqlConnection = DataFunctions.GetClientSqlConnection(client);

            // Assert
            sqlConnection.ShouldNotBeNull();
            actualConnectionString.ShouldBe(expectedConnectionString);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DataFunctionsTest_GetClientSqlConnectionForClientConnectionsNotDeployed(bool isDemo)
        {
            string actualConnectionString = null;

            // Arrange
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                { AppSettingsIsDemoKey, isDemo.ToString() },
                { AppSettingsIsNetworkDeployedKey, false.ToString() }
            };

            ShimSqlConnection.ConstructorString = (connection, connectionString) =>
                {
                    actualConnectionString = connectionString;
                };

            var expectedConnectionString = isDemo ? DemoDbConnecionString : LiveDbConnecionString;

            var client = new ClientConnections()
                             {
                                 ClientLiveDBConnectionString = LiveDbConnecionString,
                                 ClientTestDBConnectionString = DemoDbConnecionString
                             };

            // Act
            var sqlConnection = DataFunctions.GetClientSqlConnection(client);

            // Assert
            sqlConnection.ShouldNotBeNull();
            actualConnectionString.ShouldBe(expectedConnectionString);
        }
    }
}