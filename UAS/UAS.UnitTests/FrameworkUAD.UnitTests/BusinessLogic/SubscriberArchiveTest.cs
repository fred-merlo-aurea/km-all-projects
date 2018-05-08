using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Transactions.Fakes;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using EntitySubscriberArchive = FrameworkUAD.Entity.SubscriberArchive;
using KMCommonShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace FrameworkUAD.UnitTests.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberArchiveTest
    {
        private readonly SubscriberArchive _subscriberArchive = new SubscriberArchive();

        private ClientConnections _clientConnections;
        private EntitySubscriberArchive _entitySubscriberArchive;
        private StringBuilder _sbXml;

        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
            _clientConnections = new ClientConnections();
            _entitySubscriberArchive = new EntitySubscriberArchive();
            _sbXml = new StringBuilder();
        }

        [TearDown]
        public void CleanUp() => _context?.Dispose();

        [Test]
        public void SubscriberArchive_SaveSubscriberArchive_WhenClientConnectionsIsNull_ThrowsException()
        {
            // Arrange
            _clientConnections = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _subscriberArchive.SaveSubscriberArchive(_clientConnections, _entitySubscriberArchive, _sbXml);
            });
        }

        [Test]
        public void SubscriberArchive_SaveSubscriberArchive_WhenSubscriberArchiveIsNull_ThrowsException()
        {
            // Arrange
            _entitySubscriberArchive = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _subscriberArchive.SaveSubscriberArchive(_clientConnections, _entitySubscriberArchive, _sbXml);
            });
        }

        [Test]
        public void SubscriberArchive_SaveSubscriberArchive_WhenSbXmlIsNull_ThrowsException()
        {
            // Arrange
            _sbXml = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                _subscriberArchive.SaveSubscriberArchive(_clientConnections, _entitySubscriberArchive, _sbXml);
            });
        }

        [Test]
        public void SubscriberArchive_SaveSubscriberArchive_ShoudlSaveSubscriberArchive_ReturnsDoneEqualsTrue()
        {
            // Arrange
            ShimSqlDataFunctions();
            ShimTransactionScope.AllInstances.Complete = _ => { };

            // Act
            var result = _subscriberArchive.SaveSubscriberArchive(_clientConnections, _entitySubscriberArchive, _sbXml);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void SubscriberArchive_SaveSubscriberArchive_ShoudlThrowScopeExceptionAndSaveFileLog_ReturnsDoneEqualsFalse()
        {
            // Arrange
            ShimSqlDataFunctions();
            ShimTransactionScope.AllInstances.Complete = _ => throw new NullReferenceException();
            ShimTransactionScope.AllInstances.Dispose = _ => { };
            ShimFileLog.AllInstances.SaveFileLog = (_, file) => true;

            // Act
            var result = _subscriberArchive.SaveSubscriberArchive(_clientConnections, _entitySubscriberArchive, _sbXml);

            // Assert
            result.ShouldBeFalse();
        }

        private static void ShimSqlDataFunctions()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
            KMCommonShimDataFunctions.ExecuteNonQuerySqlCommand = _ => true;
        }
    }
}
