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
    /// Unit tests for <see cref="AdhocCategory"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AdhocCategoryTest
    {
        private const string CommandText = "e_AdhocCategory_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.AdhocCategory _adhocCategory;

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
            _adhocCategory = typeof(Entity.AdhocCategory).CreateInstance();

            // Act
            AdhocCategory.Save(_adhocCategory, new ClientConnections());

            // Assert
            _adhocCategory.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _adhocCategory = typeof(Entity.AdhocCategory).CreateInstance(true);

            // Act
            AdhocCategory.Save(_adhocCategory, new ClientConnections());

            // Assert
            _adhocCategory.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@CategoryID"].Value.ShouldBe(_adhocCategory.CategoryID),
                () => _saveCommand.Parameters["@CategoryName"].Value.ShouldBe((object)_adhocCategory.CategoryName ?? DBNull.Value),
                () => _saveCommand.Parameters["@SortOrder"].Value.ShouldBe(_adhocCategory.SortOrder),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_adhocCategory.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_adhocCategory.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_adhocCategory.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_adhocCategory.UpdatedByUserID ?? DBNull.Value));
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