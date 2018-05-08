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
    /// Unit tests for <see cref="History"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryTest
    {
        private const string CommandText = "e_History_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.History _history;

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
            _history = typeof(Entity.History).CreateInstance();

            // Act
            History.Save(_history, new ClientConnections());

            // Assert
            _history.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _history = typeof(Entity.History).CreateInstance(true);

            // Act
            History.Save(_history, new ClientConnections());

            // Assert
            _history.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@HistoryID"].Value.ShouldBe(_history.HistoryID),
                () => _saveCommand.Parameters["@BatchID"].Value.ShouldBe(_history.BatchID),
                () => _saveCommand.Parameters["@BatchCountItem"].Value.ShouldBe(_history.BatchCountItem),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_history.PublicationID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_history.PubSubscriptionID),
                () => _saveCommand.Parameters["@SubscriptionID"].Value.ShouldBe(_history.SubscriptionID),
                () => _saveCommand.Parameters["@HistorySubscriptionID"].Value.ShouldBe(_history.HistorySubscriptionID),
                () => _saveCommand.Parameters["@HistoryPaidID"].Value.ShouldBe(_history.HistoryPaidID),
                () => _saveCommand.Parameters["@HistoryPaidBillToID"].Value.ShouldBe(_history.HistoryPaidBillToID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_history.DateCreated),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_history.CreatedByUserID));
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