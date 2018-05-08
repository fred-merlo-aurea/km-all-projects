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
    /// Unit tests for <see cref="Adhoc"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdhocTest
    {
        private const string CommandText = "e_Adhoc_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Adhoc _adhoc;

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
            _adhoc = typeof(Entity.Adhoc).CreateInstance();

            // Act
            Adhoc.Save(_adhoc, new ClientConnections());

            // Assert
            _adhoc.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _adhoc = typeof(Entity.Adhoc).CreateInstance(true);

            // Act
            Adhoc.Save(_adhoc, new ClientConnections());

            // Assert
            _adhoc.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@AdhocID"].Value.ShouldBe(_adhoc.AdhocID),
                () => _saveCommand.Parameters["@AdhocName"].Value.ShouldBe((object)_adhoc.AdhocName ?? DBNull.Value),
                () => _saveCommand.Parameters["@CategoryID"].Value.ShouldBe(_adhoc.CategoryID),
                () => _saveCommand.Parameters["@SortOrder"].Value.ShouldBe(_adhoc.SortOrder),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_adhoc.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_adhoc.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_adhoc.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_adhoc.UpdatedByUserID ?? DBNull.Value));
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