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
    /// Unit tests for <see cref="FilterSegmentation"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterSegmentationTest
    {
        private const string CommandText = "e_FilterSegmentation_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.FilterSegmentation _filterSegmentation;

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
            _filterSegmentation = typeof(Entity.FilterSegmentation).CreateInstance();

            // Act
            FilterSegmentation.Save(_filterSegmentation, new ClientConnections());

            // Assert
            _filterSegmentation.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _filterSegmentation = typeof(Entity.FilterSegmentation).CreateInstance(true);

            // Act
            FilterSegmentation.Save(_filterSegmentation, new ClientConnections());

            // Assert
            _filterSegmentation.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterSegmentationID"].Value.ShouldBe(_filterSegmentation.FilterSegmentationID),
                () => _saveCommand.Parameters["@FilterSegmentationName"].Value.ShouldBe(_filterSegmentation.FilterSegmentationName),
                () => _saveCommand.Parameters["@Notes"].Value.ShouldBe(_filterSegmentation.Notes),
                () => _saveCommand.Parameters["@FilterID"].Value.ShouldBe(_filterSegmentation.FilterID),
                () => _saveCommand.Parameters["@IsDeleted"].Value.ShouldBe(_filterSegmentation.IsDeleted),
                () => _saveCommand.Parameters["@CreatedUserID"].Value.ShouldBe((object)_filterSegmentation.CreatedUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedDate"].Value.ShouldBe((object)_filterSegmentation.CreatedDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@UpdatedDate"].Value.ShouldBe((object)_filterSegmentation.UpdatedDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@UpdatedUserID"].Value.ShouldBe((object)_filterSegmentation.UpdatedUserID ?? DBNull.Value));
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