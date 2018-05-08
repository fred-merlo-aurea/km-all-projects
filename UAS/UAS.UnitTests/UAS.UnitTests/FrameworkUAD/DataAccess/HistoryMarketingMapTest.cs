using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="HistoryMarketingMap"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryMarketingMapTest
    {
        private const string CommandText = "e_HistoryMarketingMap_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.HistoryMarketingMap _historyMarketingMap;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _historyMarketingMap = typeof(Entity.HistoryMarketingMap).CreateInstance();

            // Act
            HistoryMarketingMap.Save(_historyMarketingMap, new ClientConnections());

            // Assert
            _historyMarketingMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _historyMarketingMap = typeof(Entity.HistoryMarketingMap).CreateInstance(true);

            // Act
            HistoryMarketingMap.Save(_historyMarketingMap, new ClientConnections());

            // Assert
            _historyMarketingMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@HistoryMarketingMapID"].Value.ShouldBe(_historyMarketingMap.HistoryMarketingMapID),
                () => _saveCommand.Parameters["@MarketingID"].Value.ShouldBe(_historyMarketingMap.MarketingID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_historyMarketingMap.PubSubscriptionID),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_historyMarketingMap.PublicationID),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_historyMarketingMap.IsActive),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_historyMarketingMap.DateCreated),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_historyMarketingMap.CreatedByUserID));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}