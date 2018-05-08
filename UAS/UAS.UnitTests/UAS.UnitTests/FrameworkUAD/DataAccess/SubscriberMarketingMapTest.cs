using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Object;
using ClientConnections = KMPlatform.Object.ClientConnections;
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="SubscriberMarketingMap"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberMarketingMapTest
    {
        private const int SubscriberId = 10;
        private const string ProcSelect = "o_SubscriberMarketingMap_Select_SubscriberID";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.SubscriberMarketingMap> _list;
        private Entity.SubscriberMarketingMap _objWithRandomValues;
        private Entity.SubscriberMarketingMap _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _objWithRandomValues = typeof(Entity.SubscriberMarketingMap).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriberMarketingMap).CreateInstance(true);

            _list = new List<Entity.SubscriberMarketingMap>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberMarketingMap.Select(SubscriberId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@SubscriberID"].Value.ShouldBe(SubscriberId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteReaderSqlCommandString = (cmd, _) =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}