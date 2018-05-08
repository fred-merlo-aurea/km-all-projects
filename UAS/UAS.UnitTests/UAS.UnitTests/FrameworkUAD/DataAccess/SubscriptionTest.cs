using System;
using System.Fakes;
using System.Data.SqlClient;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UADDataEntity = FrameworkUAD.Entity;
using KMPlatform.Object;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    [TestFixture]
    public class SubscriptionTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Save_OnException_ThrowException()
        {
            // Arrange
            var subscription = new UADDataEntity::Subscription();
            var client = new ClientConnections();
            ShimDataFunctions.ExecuteScalarSqlCommand = (cmd) => throw new Exception();
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (conn) =>
            {
                return new SqlConnection();
            };

            // Act, Assert
            Should.Throw<Exception>(() => Subscription.Save(subscription, client));
        }

        [Test]
        public void Save_OnValidCall_ReturnPositiveNumber()
        {
            // Arrange
            var subscription = new UADDataEntity::Subscription();
            var client = new ClientConnections();
            ShimDataFunctions.ExecuteScalarSqlCommand = (cmd) => 1;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = (conn) => 
            {
                return new SqlConnection();
            };

            // Act	
            var actualResult = Subscription.Save(subscription, client);

            // Assert
            actualResult.ShouldBeGreaterThan(0);
        }
    }
}
