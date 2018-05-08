using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using Batch = FrameworkUAD.DataAccess.Batch;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="Batch"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class BatchTest
    {
        private const string CommandText = "e_Batch_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Batch _batch;

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
            _batch = typeof(Entity.Batch).CreateInstance();

            // Act
            Batch.Save(_batch, new ClientConnections());

            // Assert
            _batch.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _batch = typeof(Entity.Batch).CreateInstance(true);

            // Act
            Batch.Save(_batch, new ClientConnections());

            // Assert
            _batch.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@BatchID"].Value.ShouldBe(_batch.BatchID),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_batch.PublicationID),
                () => _saveCommand.Parameters["@BatchCount"].Value.ShouldBe(_batch.BatchCount),
                () => _saveCommand.Parameters["@UserID"].Value.ShouldBe(_batch.UserID),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_batch.IsActive),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_batch.DateCreated),
                () => _saveCommand.Parameters["@DateFinalized"].Value.ShouldBe((object)_batch.DateFinalized ?? DBNull.Value),
                () => _saveCommand.Parameters["@BatchNumber"].Value.ShouldBe(_batch.BatchNumber));
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