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
    /// Unit tests for <see cref="HistoryResponseMap"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryResponseMapTest
    {
        private const string CommandText = "e_HistoryResponseMap_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.HistoryResponseMap _historyResponseMap;

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
            _historyResponseMap = typeof(Entity.HistoryResponseMap).CreateInstance();

            // Act
            HistoryResponseMap.Save(_historyResponseMap, new ClientConnections());

            // Assert
            _historyResponseMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _historyResponseMap = typeof(Entity.HistoryResponseMap).CreateInstance(true);

            // Act
            HistoryResponseMap.Save(_historyResponseMap, new ClientConnections());

            // Assert
            _historyResponseMap.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PubSubscriptionDetailID"].Value.ShouldBe(_historyResponseMap.PubSubscriptionDetailID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_historyResponseMap.PubSubscriptionID),
                () => _saveCommand.Parameters["@SubscriptionID"].Value.ShouldBe(_historyResponseMap.SubscriptionID),
                () => _saveCommand.Parameters["@CodeSheetID"].Value.ShouldBe(_historyResponseMap.CodeSheetID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_historyResponseMap.DateCreated),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_historyResponseMap.CreatedByUserID),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_historyResponseMap.ResponseOther),
                () => _saveCommand.Parameters["@HistorySubscriptionID"].Value.ShouldBe(_historyResponseMap.HistorySubscriptionID));
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