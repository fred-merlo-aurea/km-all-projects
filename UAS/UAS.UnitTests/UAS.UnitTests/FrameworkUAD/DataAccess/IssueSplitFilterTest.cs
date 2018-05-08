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
    /// Unit tests for <see cref="IssueSplitFilter"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueSplitFilterTest
    {
        private const string CommandText = "e_IssueSplitFilter_FilterDetails_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.IssueSplitFilter _issueSplitFilter;
        private DataTable _dtFilterDetails;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dtFilterDetails = new DataTable();
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
            _issueSplitFilter = typeof(Entity.IssueSplitFilter).CreateInstance();

            // Act
            IssueSplitFilter.Save(_issueSplitFilter, _dtFilterDetails, new ClientConnections());

            // Assert
            _issueSplitFilter.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _issueSplitFilter = typeof(Entity.IssueSplitFilter).CreateInstance(true);

            // Act
            IssueSplitFilter.Save(_issueSplitFilter, _dtFilterDetails, new ClientConnections());

            // Assert
            _issueSplitFilter.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@FilterID"].Value.ShouldBe(_issueSplitFilter.FilterID),
                () => _saveCommand.Parameters["@PubId"].Value.ShouldBe(_issueSplitFilter.PubId),
                () => _saveCommand.Parameters["@FilterName"].Value.ShouldBe(_issueSplitFilter.FilterName),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_issueSplitFilter.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe(_issueSplitFilter.UpdatedByUserID),
                () => _saveCommand.Parameters["@TVP_IssueSplitFilterDetails"].Value.ShouldBe(_dtFilterDetails));
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