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
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="MarketingMap"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MarketingMapTest
    {
        private const string CommandText = "e_MarketingMap_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.MarketingMap _marketingMap;

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
            _marketingMap = typeof(Entity.MarketingMap).CreateInstance();

            // Act
            MarketingMap.Save(_marketingMap, new ClientConnections());

            // Assert
            _marketingMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _marketingMap = typeof(Entity.MarketingMap).CreateInstance(true);

            // Act
            MarketingMap.Save(_marketingMap, new ClientConnections());

            // Assert
            _marketingMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@MarketingID"].Value.ShouldBe(_marketingMap.MarketingID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_marketingMap.PubSubscriptionID),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_marketingMap.PublicationID),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_marketingMap.IsActive),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_marketingMap.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_marketingMap.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_marketingMap.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_marketingMap.UpdatedByUserID ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return true;
            };
        }
    }
}